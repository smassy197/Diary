Imports Google.Cloud.Firestore
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Text
Imports FirebaseAdmin
Imports FirebaseAdmin.Auth
Imports Google.Apis.Auth.OAuth2

Public Class Form4
    Private databasePath As String
    Private backupFilePath As String

    Public Sub New(ByVal path As String)
        ' Questa chiamata è richiesta dal designer.
        InitializeComponent()
        ' Assegna il valore di path alla variabile di classe databasePath
        databasePath = path
    End Sub

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Impostazione dei ToolTips
        Dim tooltip As New ToolTip()
        tooltip.SetToolTip(Me.btnBackup, "Clicca qui per eseguire un backup del tuo database.")
        tooltip.SetToolTip(Me.btnCarica, "Utilizza questa opzione per ripristinare il database da un backup precedente.")

    End Sub

    Private Async Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click
        Dim firebaseConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json")

        If File.Exists(firebaseConfigFilePath) Then
            If FirebaseApp.DefaultInstance Is Nothing Then
                Try
                    Dim options = New AppOptions() With {
                    .Credential = GoogleCredential.FromFile(firebaseConfigFilePath)
                }
                    FirebaseApp.Create(options)
                    Dim firebaseAuth As FirebaseAuth = FirebaseAuth.DefaultInstance

                    Console.WriteLine("Firebase configurato con successo.")
                Catch ex As Exception
                    Console.WriteLine("Errore durante l'inizializzazione di FirebaseApp: " & ex.Message)
                    Return
                End Try
            End If

            Try
                Dim projectId = GetProjectIdFromJson(firebaseConfigFilePath)
                Dim db = FirestoreDb.Create(projectId)

                ' Percorso della cartella "Downloads"
                Dim downloadsFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
                Dim backupFileName As String = "firebase_backup_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".json"
                Dim backupFilePath As String = Path.Combine(downloadsFolder, backupFileName)

                ' Recupera tutte le note
                Dim notesCollection = Await db.Collection("notes").GetSnapshotAsync()
                Console.WriteLine("Recupero delle note completato.")

                ' Crea una lista di dizionari per serializzare i dati
                Dim notesList As New List(Of Dictionary(Of String, Object))
                For Each documentSnapshot In notesCollection.Documents
                    Dim noteData = documentSnapshot.ToDictionary()

                    ' Converti DataFirebase in una stringa
                    If noteData.ContainsKey("Date") AndAlso TypeOf noteData("Date") Is Timestamp Then
                        noteData("Date") = CType(noteData("Date"), Timestamp).ToDateTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
                    End If

                    notesList.Add(noteData)
                Next

                ' Serializza la lista in JSON e salva il file
                Dim json As String = JsonConvert.SerializeObject(notesList, Formatting.Indented)
                File.WriteAllText(backupFilePath, json)
                Console.WriteLine($"Backup creato con successo come {backupFilePath}!")

                MessageBox.Show($"Backup creato con successo come {backupFilePath}!", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                Console.WriteLine($"Errore durante la creazione del backup: {ex.Message}")
                MessageBox.Show($"Errore durante la creazione del backup: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            Console.WriteLine("Il file di configurazione non è stato trovato.")
        End If
    End Sub

    Private Sub btnApri_Click(sender As Object, e As EventArgs) Handles btnApri.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "JSON Files|*.json"
            ofd.Title = "Seleziona il file di backup del database"

            ' Imposta la cartella "Downloads" come cartella iniziale
            ofd.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")

            If ofd.ShowDialog() = DialogResult.OK Then
                ' Memorizza il percorso del file di backup
                backupFilePath = ofd.FileName

                ' Visualizza il nome del file di backup nella label
                lblBackupFileName.Text = $"File di backup selezionato: {Path.GetFileName(backupFilePath)}"
                Console.WriteLine($"File di backup selezionato: {Path.GetFileName(backupFilePath)}")
            End If
        End Using
    End Sub

    Private Async Sub btnCarica_Click(sender As Object, e As EventArgs) Handles btnCarica.Click
        If String.IsNullOrEmpty(backupFilePath) Then
            MessageBox.Show("Per favore, seleziona un file di backup prima di caricare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Leggi il file di backup
            Dim json As String = File.ReadAllText(backupFilePath)
            Dim notesList As List(Of Dictionary(Of String, Object)) = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json)
            Console.WriteLine("File di backup caricato e deserializzato.")

            ' Ottieni il projectId
            Dim projectId = GetProjectIdFromJson(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))
            Console.WriteLine("Project ID ottenuto: " & projectId)

            ' Crea una connessione a Firestore
            Console.WriteLine("Creazione della connessione a Firestore...")
            Dim db = FirestoreDb.Create(projectId)
            Console.WriteLine("Connessione a Firestore creata con successo.")

            ' Pulisce la collezione "notes"
            Dim notesCollection = db.Collection("notes")
            Dim querySnapshot = Await notesCollection.GetSnapshotAsync()
            Console.WriteLine("Pulizia della collezione 'notes' in corso...")

            For Each documentSnapshot In querySnapshot.Documents
                Await documentSnapshot.Reference.DeleteAsync()
            Next
            Console.WriteLine("Collezione 'notes' pulita.")

            ' Ripristina i dati nel database Firestore
            For Each note In notesList
                Dim docId As String = If(note.ContainsKey("id"), note("id").ToString(), Guid.NewGuid().ToString())

                ' Converti DataFirebase da stringa a Timestamp
                If note.ContainsKey("Date") Then
                    note("Date") = Timestamp.FromDateTime(DateTime.Parse(note("Date").ToString()).ToUniversalTime())
                End If

                Dim docRef = db.Collection("notes").Document(docId)
                Await docRef.SetAsync(note, SetOptions.MergeAll) ' Usa SetOptions.MergeAll per aggiornare il documento esistente
                Console.WriteLine($"Documento con ID {docId} caricato.")
            Next

            MessageBox.Show("Database aggiornato con successo!", "Aggiornamento", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Console.WriteLine("Database aggiornato con successo!")
        Catch ex As Exception
            Console.WriteLine($"Errore durante l'aggiornamento del database: {ex.Message}")
            MessageBox.Show($"Errore durante l'aggiornamento del database: {ex.Message}", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetProjectIdFromJson(filePath As String) As String
        If Not File.Exists(filePath) Then
            Console.WriteLine("Il file JSON non esiste.")
            MessageBox.Show("Il file JSON non esiste.")
            Return Nothing
        End If

        Try
            Dim json = File.ReadAllText(filePath)
            Dim jsonObject = JObject.Parse(json)
            Dim projectId = jsonObject("project_id")?.ToString()
            Console.WriteLine("Project ID letto dal file JSON: " & projectId)
            Return projectId
        Catch ex As Exception
            Console.WriteLine("Errore nella lettura del file JSON: " & ex.Message)
            MessageBox.Show("Errore nella lettura del file JSON: " & ex.Message)
            Return Nothing
        End Try
    End Function
End Class
