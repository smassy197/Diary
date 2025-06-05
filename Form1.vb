
Imports System.Drawing.Text
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports WeCantSpell.Hunspell
Imports FirebaseAdmin
Imports Google.Apis.Auth.OAuth2
Imports Google.Cloud.Firestore
Imports System.Net
Imports FirebaseAdmin.Auth
Imports System.Threading.Tasks
Imports Newtonsoft.Json.Linq

Imports Google.Cloud.Firestore.V1
Imports Google.Cloud.Firestore.V1.StructuredQuery.Types
Imports System.Reflection.Metadata
Imports Krypton.Toolkit
Imports Newtonsoft.Json
' Inserisci questi import all'inizio del file
Imports System.Diagnostics
Imports System.Net.Http


Public Class Form1
    ' Aggiungi queste costanti in Form1
    Private Const VersionUrl As String = "https://raw.githubusercontent.com/smassy197/Diary/master/version.txt"
    Private Const ExeUrl As String = "https://github.com/smassy197/Diary/releases/latest/download/mysetup_Diary.exe"
    Private Const LocalExePath As String = "Diary.exe" ' Percorso dell'eseguibile locale

    Private databasePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "diary.db")
    Private connectionString As String = $"Data Source={databasePath};Version=3;"
    Private configFilePath As String = Path.Combine(Path.GetDirectoryName(databasePath), "userconfig.txt")
    Private userName As String

    'firebase ---------------------------------------------------
    Private firebaseApp As FirebaseApp
    Private firebaseAuth As FirebaseAuth
    Private firebaseConfigFilePath As String
    Private credentialsFilePath As String

    'firebase ---------------------------------------------------

    ' Dichiarazione della TextBox per la password
    Private txtPassword As New TextBox()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'OpenConsole()
        ' Inizializza le variabili
        Dim directoryPath As String = Path.GetDirectoryName(databasePath)
        Dim configFilePath As String = Path.Combine(directoryPath, "userconfig.txt")
        Dim isNewlyCreated As Boolean = False ' Una variabile per verificare se la cartella è stata appena creata
        Dim userName As String = ""

        ' Verifica se la cartella esiste, altrimenti creala
        If Not Directory.Exists(directoryPath) Then
            Directory.CreateDirectory(directoryPath)
            If Directory.Exists(directoryPath) Then
                isNewlyCreated = True ' La cartella è stata appena creata
            End If
        End If

        ' Visualizza messaggi sulla creazione della cartella se necessario
        If isNewlyCreated Then
            MessageBox.Show($"Verifica esistenza cartella: {directoryPath}")
            MessageBox.Show("La cartella non esiste. Tentativo di creazione...")
            MessageBox.Show("Cartella creata con successo!")
        End If

        ' Se il file di configurazione non esiste, richiedi il nome utente e la password e salvali nel file
        If Not File.Exists(configFilePath) Then
            userName = InputBox("Inserisci il tuo nome:", "Prima Installazione", Environment.UserName)
            'Dim password As String = InputBox("Inserisci la password:", "Prima Installazione", "")

            ' Nel metodo Form1_Load, sostituisci l'uso di InputBox con la tua TextBox
            Dim password As String = GetPassword()

            ' Salva il nome utente e la password nel file di configurazione
            SaveConfiguration(configFilePath, userName, password)

            ' Procedi con l'inizializzazione dell'app
            InitializeApp(userName)
        Else
            ' Se il file di configurazione esiste, verifica la password
            If File.Exists(configFilePath) Then

                ' Leggi il nome utente dal file di configurazione
                userName = ReadUserNameFromConfig(configFilePath)

                CheckPassword(configFilePath)
                ' La password è corretta, procedi con l'inizializzazione dell'app
                InitializeApp(userName)


            Else
                ' La password non è corretta o il nome utente non è stato letto correttamente, chiudi l'applicazione
                MessageBox.Show("Password non corretta. Chiusura dell'applicazione.")
                Me.Close()
            End If

        End If


    End Sub

    ' Funzione per ottenere la password tramite la TextBox
    Private Function GetPassword() As String
        Dim passwordForm As New Form()




        ' Imposta le proprietà del form della password
        passwordForm.Text = "Inserisci la password"
        passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog
        passwordForm.MaximizeBox = False
        passwordForm.MinimizeBox = False
        passwordForm.StartPosition = FormStartPosition.CenterScreen

        ' Imposta la TextBox della password
        txtPassword.PasswordChar = "*"
        txtPassword.Width = 250
        txtPassword.Location = New Point(10, 10)
        passwordForm.Controls.Add(txtPassword)

        ' Aggiungi un pulsante "OK" per confermare la password
        Dim btnOK As New Button()
        btnOK.Text = "OK"
        btnOK.DialogResult = DialogResult.OK
        btnOK.Size = New Size(100, 30)
        btnOK.Location = New Point(10, 60)
        passwordForm.Controls.Add(btnOK)
        ' Imposta il pulsante "OK" come pulsante predefinito del form
        passwordForm.AcceptButton = btnOK

        ' Aggiungi un pulsante "Annulla" per chiudere la finestra senza confermare la password
        Dim btnCancel As New Button()
        btnCancel.Text = "Annulla"
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Size = New Size(100, 30)
        btnCancel.Location = New Point(120, 60)
        passwordForm.Controls.Add(btnCancel)

        ' Imposta le dimensioni della finestra
        passwordForm.ClientSize = New Size(300, 150)

        ' Mostra il form e restituisci la password
        If passwordForm.ShowDialog() = DialogResult.OK Then
            Return txtPassword.Text
        Else
            Return ""
        End If
    End Function

    Private Function CheckPassword(configFilePath As String) As Boolean
        Try
            ' Leggi il contenuto del file di configurazione
            Dim lines() As String = File.ReadAllLines(configFilePath)

            ' Verifica se il file contiene almeno due righe (username e password)
            If lines.Length >= 2 Then
                ' Estrai la password dalla seconda riga
                Dim storedPasswordLine As String = lines(1)
                Dim storedPassword As String = storedPasswordLine.Substring("password: ".Length).Trim()

                ' Chiedi all'utente di inserire la password
                ' Dim enteredPassword As String = InputBox("Inserisci la password:", "Password", "")
                Dim enteredPassword As String = GetPassword()


                ' Confronta la password inserita con quella salvata
                If enteredPassword = storedPassword Then
                    ' La password è corretta
                    Return True
                Else
                    ' La password non è corretta
                    MessageBox.Show("Password non corretta. Chiusura dell'applicazione.")
                    Me.Close()
                    Return False
                End If
            Else
                ' Il file di configurazione non contiene le informazioni necessarie
                MessageBox.Show("Il file di configurazione non è valido. Chiusura dell'applicazione.")
                Me.Close()
                Return False
            End If
        Catch ex As Exception
            ' Gestisci eventuali eccezioni durante la lettura del file
            MessageBox.Show($"Errore durante la lettura del file di configurazione: {ex.Message}. Chiusura dell'applicazione.")
            Me.Close()
            Return False
        End Try
    End Function

    Private Function ReadUserNameFromConfig(configFilePath As String) As String
        Try
            ' Leggi il contenuto del file di configurazione
            Dim lines() As String = File.ReadAllLines(configFilePath)

            ' Verifica se il file contiene almeno una riga
            If lines.Length >= 1 Then
                ' Estrai il nome utente dalla prima riga
                Dim userNameLine As String = lines(0)
                Return userNameLine.Substring("username: ".Length).Trim()
            Else
                ' Il file di configurazione non contiene le informazioni necessarie
                MessageBox.Show("Il file di configurazione non è valido. Chiusura dell'applicazione.")
                Me.Close()
                Return ""
            End If
        Catch ex As Exception
            ' Gestisci eventuali eccezioni durante la lettura del file
            MessageBox.Show($"Errore durante la lettura del file di configurazione: {ex.Message}. Chiusura dell'applicazione.")
            Me.Close()
            Return ""
        End Try
    End Function

    ' Salva il nome utente e la password nel file di configurazione
    Private Sub SaveConfiguration(configFilePath As String, userName As String, password As String)
        ' Salva il nome utente e la password nel file di configurazione
        File.WriteAllText(configFilePath, $"username: {userName}{Environment.NewLine}password: {password}")
    End Sub

    Private Async Sub InitializeApp(userName As String)

        ' Imposta il titolo della finestra
        Me.Text = $"Diario di {userName}"

        ' Imposta la data iniziale del MonthCalendar alla data odierna
        Dim today As DateTime = DateTime.Now
        MonthCalendar1.SelectionStart = today
        MonthCalendar1.SelectionEnd = today ' Aggiungi questa riga per selezionare una singola data

        'firebase auth automatico--------------------------------------------------------------------------
        ' Configura il Timer
        connectionCheckTimer.Interval = 60000 ' Controlla ogni minuto (60000 ms)
        AddHandler connectionCheckTimer.Tick, AddressOf CheckConnection
        connectionCheckTimer.Start()


        Dim firebaseConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json")

        lblStatus.Text = "Controllo connessione..."
        lblStatus.ForeColor = Color.Black

        If File.Exists(firebaseConfigFilePath) Then
            If firebaseApp Is Nothing Then
                Try
                    Dim options = New AppOptions() With {
                    .Credential = GoogleCredential.FromFile(firebaseConfigFilePath)
                }
                    firebaseApp = FirebaseApp.Create(options)
                    firebaseAuth = FirebaseAuth.GetAuth(firebaseApp)
                    projectId = GetProjectIdFromJson(firebaseConfigFilePath)
                    '  MessageBox.Show("Firebase configurato con successo." & vbCrLf & "Project ID: " & projectId)
                Catch ex As Exception
                    lblStatus.Text = "Errore di configurazione"
                    lblStatus.ForeColor = Color.Red
                    Return
                End Try
            End If

            Try
                Await VerificaStatoFirebase(projectId)
            Catch ex As Exception
                lblStatus.Text = "Offline"
                lblStatus.ForeColor = Color.Red
            End Try
        Else
            lblStatus.Text = "File di configurazione non trovato"
            lblStatus.ForeColor = Color.Red
        End If
        '--------------------------------------------------------------------------------------------------


        ' Al caricamento del form, popola la ListBox con le voci del diario

        AggiornaCalendario()
        CheckForUpdates()

    End Sub

    Private Async Sub CheckConnection(sender As Object, e As EventArgs)
        Dim firebaseConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json")
        Try
            lblStatus.Text = "Controllo connessione..."
            lblStatus.ForeColor = Color.Black

            If File.Exists(firebaseConfigFilePath) Then
                If firebaseApp IsNot Nothing Then
                    Await VerificaStatoFirebase(projectId)
                Else
                    lblStatus.Text = "Errore di configurazione"
                    lblStatus.ForeColor = Color.Red
                End If
            Else
                lblStatus.Text = "File di configurazione non trovato"
                lblStatus.ForeColor = Color.Red
            End If
        Catch ex As Exception
            lblStatus.Text = "Offline"
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        connectionCheckTimer.Stop()
    End Sub

    '___________________________________________ update version e download exe
    ' Funzione per ottenere la versione locale
    Private Function GetLocalVersion() As String
        Return Application.ProductVersion
    End Function

    ' Funzione per ottenere la versione remota da GitHub
    Private Async Function GetRemoteVersion() As Task(Of String)
        Using client As New HttpClient()
            Return Await client.GetStringAsync(VersionUrl)
        End Using
    End Function

    ' Funzione per confrontare le versioni (ritorna True se la remota è più recente)
    Private Function IsNewVersionAvailable(localVersion As String, remoteVersion As String) As Boolean
        Try
            Dim vLocal = New Version(localVersion.Trim())
            Dim vRemote = New Version(remoteVersion.Trim())
            Return vRemote > vLocal
        Catch
            Return False
        End Try
    End Function

    ' Funzione per scaricare il nuovo exe
    Private Async Function DownloadNewExe() As Task
        Dim tempPath = Path.Combine(Path.GetTempPath(), "Diary_update.exe")
        Using client As New HttpClient()
            Using response = Await client.GetAsync(ExeUrl, HttpCompletionOption.ResponseHeadersRead)
                response.EnsureSuccessStatusCode()
                Using fs = New FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None)
                    Await response.Content.CopyToAsync(fs)
                End Using
            End Using
        End Using
        Process.Start(tempPath)
        Application.Exit()
    End Function

    ' Chiamala all'avvio o con un pulsante
    Private Async Sub CheckForUpdates()
        Try
            Dim localVersion = GetLocalVersion()
            Dim remoteVersion = Await GetRemoteVersion()
            If IsNewVersionAvailable(localVersion, remoteVersion) Then
                If MessageBox.Show($"È disponibile una nuova versione ({remoteVersion.Trim()}). Vuoi aggiornare ora?", "Aggiornamento disponibile", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                    Await DownloadNewExe()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Errore durante la verifica aggiornamenti: " & ex.Message)
        End Try
    End Sub


    '-------------------------------------------------------------------------


    Private Async Sub btnAggiungiVoceFirestore_Click(sender As Object, e As EventArgs) Handles btnAggiungiVoceFirestore.Click
        Try
            If dtpData Is Nothing Then
                MessageBox.Show("dptData non è inizializzato correttamente.")
                Return
            End If

            If txtTitolo.Text = "" Then
                MessageBox.Show("Inserisci un titolo prima di aggiungere la voce.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If rtbContenuto.Text = "" Then
                MessageBox.Show("Scrivi un contenuto prima di aggiungere la voce.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim data As DateTime

            ' Usa la data del MonthCalendar se la sua visibilità è True, altrimenti usa dtpData
            If MonthCalendar1.Visible Then
                data = MonthCalendar1.SelectionStart
            Else
                data = dtpData.Value
            End If


            Dim titolo = txtTitolo.Text
            Dim nota = rtbContenuto.Text

            Console.WriteLine($"Data: {data}, Titolo: {titolo}, Contenuto: {nota}")

            Dim projectId = GetProjectIdFromJson(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))

            Dim progress As New ProgressForm()
            progress.Show()

            Try
                ' Verifica se l'app è online o offline
                If lblStatus.Text = "Online" Then
                    If lstFirebase.SelectedIndex >= 0 Then
                        ' Modifica della voce esistente su Firestore
                        Dim selectedItem = DirectCast(lstFirebase.SelectedItem, String)
                        Dim documentId As String = Nothing

                        For Each key In voceDictionary.Keys
                            Dim voce = voceDictionary(key)
                            If $"{voce.DataFirebase.ToShortDateString()}: {voce.Title}" = selectedItem Then
                                documentId = key
                                Exit For
                            End If
                        Next

                        If Not String.IsNullOrEmpty(documentId) AndAlso Not String.IsNullOrEmpty(projectId) Then
                            Await ModificaVoceSuFirestore(documentId, data, titolo, nota, projectId)
                        Else
                            MessageBox.Show("Impossibile trovare il documento o il Project ID.")
                        End If
                    Else
                        ' Aggiunta di una nuova voce su Firestore
                        If Not String.IsNullOrEmpty(projectId) Then

                            Await AggiungiVoceSuFirestore(data, titolo, nota, projectId)
                        Else
                            MessageBox.Show("Impossibile ottenere il Project ID dal file JSON di configurazione.")
                        End If
                    End If

                    ' Aggiorna le note dopo l'operazione su Firestore
                    Await CaricaVociDaFirestore(projectId)
                Else
                    ' Gestione offline: salva la voce nella cache
                    Dim dataUtc As DateTime = data.ToUniversalTime()
                    Dim nuovaVoce As New Dictionary(Of String, Object) From {
                    {"Title", titolo},
                    {"Content", nota},
                    {"DataFirebase", dataUtc},
                    {"Label", txtFlag.Text},
                    {"PCName", Environment.MachineName}
                }

                    SalvaNoteInCache(nuovaVoce)

                    MessageBox.Show("Voce salvata offline. Verrà sincronizzata quando la connessione sarà ripristinata.", "Offline", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ' Pulisci i campi
                txtTitolo.Text = ""
                rtbContenuto.Clear()
                txtFlag.Text = ""
                lblPCName.Text = ""

                Console.WriteLine("Aggiornamento delle ListBox e del calendario.")
                AggiornaCalendario()
                Await CaricaVociDaFirestore(projectId)

            Catch ex As Exception
                MessageBox.Show($"Errore durante l'operazione: {ex.Message}")
            Finally
                progress.Close()
            End Try

        Catch ex As NullReferenceException
            MessageBox.Show("Si è verificato un errore di riferimento nullo: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Si è verificato un errore: " & ex.Message)
        End Try
    End Sub



    Private projectId As String

    Private Async Sub btnEliminaVoceFirestore_Click(sender As Object, e As EventArgs) Handles btnEliminaVoceFirestore.Click
        ' Verifica se una voce è selezionata
        If lstFirebase.SelectedIndex >= 0 Then
            Dim selectedItem = DirectCast(lstFirebase.SelectedItem, String)
            Dim voceSelezionata As VoceFirebase = Nothing
            Dim documentId As String = Nothing

            ' Trova la chiave corrispondente
            For Each key In voceDictionary.Keys
                Dim voce = voceDictionary(key)
                If $"{voce.DataFirebase.ToShortDateString()}: {voce.Title}" = selectedItem Then
                    voceSelezionata = voce
                    documentId = key
                    Exit For
                End If
            Next

            If voceSelezionata IsNot Nothing AndAlso documentId IsNot Nothing Then
                Dim confermaEliminazione = MessageBox.Show($"Sei sicuro di voler eliminare la voce selezionata?{Environment.NewLine}Titolo: {voceSelezionata.Title}", "Conferma eliminazione", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If confermaEliminazione = DialogResult.Yes Then
                    ' Esegui l'eliminazione dalla ListBox
                    Console.WriteLine($"Eliminazione della voce selezionata dalla ListBox: {voceSelezionata.Title}")
                    lstFirebase.Items.RemoveAt(lstFirebase.SelectedIndex)

                    ' Esegui l'eliminazione dal database Firestore
                    Dim firebaseConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json")
                    projectId = GetProjectIdFromJson(firebaseConfigFilePath)
                    Console.WriteLine($"Eliminazione dal Firestore - Document ID: {documentId}, Project ID: {projectId}")
                    Await EliminaVoceSuFirestore(documentId, projectId)

                    ' Pulisci i campi di input
                    txtTitolo.Text = ""
                    rtbContenuto.Clear()
                    txtFlag.Text = ""
                    lblPCName.Text = ""


                    ' Aggiorna le voci da Firestore
                    Console.WriteLine("Aggiornamento delle voci da Firestore.")
                    Await CaricaVociDaFirestore(projectId)
                End If
            Else
                MessageBox.Show("Seleziona una voce da eliminare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("Seleziona una voce da eliminare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        Console.WriteLine("Aggiornamento del calendario.")
        AggiornaCalendario()
    End Sub


    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ' Chiedi conferma prima di eseguire il reset
        Dim confermaReset As DialogResult = MessageBox.Show("Sei sicuro di voler resettare i campi?", "Conferma Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confermaReset = DialogResult.Yes Then
            ' Pulisci i campi di input 
            txtTitolo.Text = ""
            lblDataNota.Text = ""
            rtbContenuto.Clear()
            lstFirebase.Items.Clear()
        End If
    End Sub

    Private Sub btnMostraForm4_Click(sender As Object, e As EventArgs) Handles btnMostraForm4.Click
        Dim form3 As New Form4(databasePath)
        form3.ShowDialog()
    End Sub


    Private Sub btnIngrandisci_Click(sender As Object, e As EventArgs) Handles btnIngrandisci.Click
        ' Aumenta la dimensione del carattere di 1
        rtbContenuto.Font = New Font(rtbContenuto.Font.FontFamily, rtbContenuto.Font.Size + 1)
    End Sub

    Private Sub btnDiminuisci_Click(sender As Object, e As EventArgs) Handles btnDiminuisci.Click
        ' Riduci la dimensione del carattere di 1
        rtbContenuto.Font = New Font(rtbContenuto.Font.FontFamily, rtbContenuto.Font.Size - 1)
    End Sub

    Private Sub btnCheckSpelling_Click(sender As Object, e As EventArgs) Handles btnCheckSpelling.Click
        ' Elimina gli spazi multipli
        ReplaceAllDoubleSpaces(rtbContenuto)
        'ReplaceAllDoubleSpaces(txtTitolo)
        CorrectCapitalizationAfterPeriods()
        ' Ora evidenzia le parole errate sul testo aggiornato
        HighlightMisspelledWords()
    End Sub

    Private Sub HighlightMisspelledWords()
        Dim checker As New SpellChecker()
        Dim misspelledWords = checker.CheckSpelling(rtbContenuto.Text)

        ' Reset formatting
        rtbContenuto.SelectAll()
        rtbContenuto.SelectionBackColor = Color.White
        rtbContenuto.SelectionColor = Color.Black
        rtbContenuto.DeselectAll()

        For Each word In misspelledWords
            Dim start As Integer = 0
            Do
                start = rtbContenuto.Find(word, start, RichTextBoxFinds.None)
                If start >= 0 Then
                    rtbContenuto.SelectionBackColor = Color.Yellow
                    rtbContenuto.SelectionColor = Color.Red
                    start += word.Length
                End If
            Loop Until start < 0 OrElse start >= rtbContenuto.TextLength
        Next
    End Sub
    Private Sub rtbContenuto_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbContenuto.MouseUp
        If e.Button = MouseButtons.Right Then
            Dim charIndex = rtbContenuto.GetCharIndexFromPosition(e.Location)
            If charIndex >= 0 AndAlso charIndex < rtbContenuto.Text.Length Then
                Dim clickedChar = rtbContenuto.Text(charIndex)

                If Char.IsWhiteSpace(clickedChar) Then
                    ' User clicked on a whitespace
                    If MessageBox.Show("Vuoi eliminare tutti gli spazi multipli tra le parole?", "Conferma", MessageBoxButtons.OKCancel) = DialogResult.OK Then
                        ReplaceAllDoubleSpaces(rtbContenuto)
                    End If
                    Return
                End If
            End If

            Dim clickedWord As String = GetWordAtPoint(rtbContenuto, e.Location)
            If String.IsNullOrWhiteSpace(clickedWord) Then Return

            Dim checker As New SpellChecker()

            ' Reset context menu
            rtbContenuto.ContextMenuStrip = New ContextMenuStrip()

            ' Check for misspelled word
            If Not checker.CheckSingleWord(clickedWord) Then
                Dim contextMenu As New ContextMenuStrip()

                ' Opzione per aggiungere la parola al dizionario
                Dim addToDictionaryItem As New ToolStripMenuItem("Aggiungi al dizionario")
                AddHandler addToDictionaryItem.Click, Sub(s, ev)
                                                          checker.AddToUserDictionary(clickedWord)
                                                          HighlightMisspelledWords()
                                                      End Sub
                contextMenu.Items.Add(addToDictionaryItem)
                contextMenu.Items.Add(New ToolStripSeparator())

                ' Suggerimenti di correzione
                Dim suggestions = checker.GetSuggestions(clickedWord)
                For Each suggestion In suggestions
                    Dim menuItem As New ToolStripMenuItem(suggestion)
                    AddHandler menuItem.Click, Sub(s, ev) ReplaceWordAtPoint(rtbContenuto, e.Location, suggestion)
                    contextMenu.Items.Add(menuItem)
                Next

                rtbContenuto.ContextMenuStrip = contextMenu
                rtbContenuto.ContextMenuStrip.Show(rtbContenuto, e.Location)
            End If
        End If

    End Sub
    Private Function GetWordAtPoint(richTextBox As RichTextBox, location As Point) As String
        Dim index = richTextBox.GetCharIndexFromPosition(location)
        If index < 0 OrElse index >= richTextBox.Text.Length Then Return String.Empty
        Dim start = richTextBox.Text.LastIndexOf(" "c, index) + 1
        If start < 0 Then start = 0
        Dim endChar = richTextBox.Text.IndexOf(" "c, index)
        If endChar < 0 Then endChar = richTextBox.Text.Length
        Return richTextBox.Text.Substring(start, endChar - start)
    End Function
    Private Sub ReplaceWordAtPoint(richTextBox As RichTextBox, location As Point, replacementWord As String)
        Dim index = richTextBox.GetCharIndexFromPosition(location)
        If index < 0 OrElse index >= richTextBox.Text.Length Then Return

        Dim start = richTextBox.Text.LastIndexOf(" "c, index) + 1
        If start < 0 Then start = 0

        Dim endChar = richTextBox.Text.IndexOf(" "c, index)
        If endChar < 0 Then endChar = richTextBox.Text.Length

        richTextBox.Select(start, endChar - start)
        richTextBox.SelectedText = replacementWord

        ' Reset the style
        richTextBox.SelectionStart = start
        richTextBox.SelectionLength = replacementWord.Length
        richTextBox.SelectionBackColor = Color.White
        richTextBox.SelectionColor = Color.Black
    End Sub

    Private Sub ReplaceAllDoubleSpaces(richTextBox As RichTextBox)
        richTextBox.Text = Regex.Replace(richTextBox.Text, "\s{2,}", " ")
    End Sub

    Private Sub CorrectCapitalizationAfterPeriods()
        Dim pattern As String = "(\.\s*)([a-z])"
        rtbContenuto.Text = Regex.Replace(rtbContenuto.Text, pattern, Function(m) m.Groups(1).Value & Char.ToUpper(m.Groups(2).Value(0)))

        ' Rendi maiuscolo l'inizio del testo, se è minuscolo
        If rtbContenuto.Text.Length > 0 AndAlso Char.IsLower(rtbContenuto.Text(0)) Then
            rtbContenuto.Text = Char.ToUpper(rtbContenuto.Text(0)) & rtbContenuto.Text.Substring(1)
        End If
    End Sub

    Private Sub btnStampa_Click(sender As Object, e As EventArgs) Handles btnStampa.Click
        StampaContenuto()
    End Sub

    Private Sub StampaContenuto()
        ' Creazione di un oggetto PrintDocument
        Dim pd As New Printing.PrintDocument()

        ' Associa l'evento PrintPage al gestore di eventi per la stampa
        AddHandler pd.PrintPage, AddressOf PrintPageHandler

        ' Mostra la finestra di dialogo di stampa
        Dim printDialog As New PrintDialog()
        printDialog.Document = pd

        If printDialog.ShowDialog() = DialogResult.OK Then
            ' Avvia la stampa
            pd.Print()
        End If
    End Sub

    Private Sub PrintPageHandler(sender As Object, e As Printing.PrintPageEventArgs)
        ' Aggiungi più margine in alto
        Dim topMargin As Integer = 100
        Dim margin As Integer = 50
        Dim yPos As Integer = topMargin

        ' Estrai il contenuto da stampare
        Dim titolo As String = txtTitolo.Text

        ' Converti l'immagine da PictureBox a Bitmap



        Dim contenuto As String = rtbContenuto.Text

        ' Font per il titolo
        Dim titoloFont As New Font("Arial", 14, FontStyle.Bold)

        ' Font per il contenuto
        Dim contenutoFont As New Font("Arial", 12)

        ' Margini
        Dim linesPerPage As Integer = 0
        Dim count As Integer = 0

        ' Calcola il numero di linee per pagina
        linesPerPage = e.MarginBounds.Height / contenutoFont.GetHeight(e.Graphics)

        ' Disegna il titolo
        e.Graphics.DrawString(titolo, titoloFont, Brushes.Black, New PointF(margin, yPos))
        yPos += titoloFont.GetHeight(e.Graphics)


        ' Disegna il contenuto con word wrap e giustifica il testo
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Near
        format.LineAlignment = StringAlignment.Near
        format.Trimming = StringTrimming.Word
        format.FormatFlags = StringFormatFlags.LineLimit

        Dim rect As New RectangleF(margin, yPos, e.PageSettings.PrintableArea.Width - 2 * margin, e.PageSettings.PrintableArea.Height - yPos)

        ' Divide il contenuto in paragrafi
        Dim paragraphs As String() = contenuto.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

        ' Stampa i paragrafi con giustificazione
        For Each paragraph As String In paragraphs
            Dim lineRect As New RectangleF(rect.X, yPos, rect.Width, rect.Height)
            e.Graphics.DrawString(paragraph, contenutoFont, Brushes.Black, lineRect, format)
            yPos += contenutoFont.GetHeight(e.Graphics)
        Next

        ' Verifica se ci sono più pagine
        If yPos < e.PageSettings.PrintableArea.Height Then
            e.HasMorePages = False
        Else
            e.HasMorePages = True
        End If

        ' Divide il contenuto in linee
        Dim lines As String() = contenuto.Split(New Char() {ControlChars.CrLf}, StringSplitOptions.RemoveEmptyEntries)

        ' Stampa le linee
        For Each line As String In lines
            ' Calcola quanti caratteri possono essere stampati in una riga
            Dim charsFit As Integer
            Dim linesFilled As Integer
            Dim charsRemaining As Integer = line.Length
            Dim y As Single = yPos + count * contenutoFont.GetHeight(e.Graphics)

            ' Stampa il testo in più righe se necessario
            While charsRemaining > 0
                e.Graphics.MeasureString(line, contenutoFont, New SizeF(rect.Width, rect.Height), format, charsFit, linesFilled)
                e.Graphics.DrawString(line.Substring(0, charsFit), contenutoFont, Brushes.Black, rect, format)
                line = line.Substring(charsFit)
                charsRemaining -= charsFit
                yPos += contenutoFont.GetHeight(e.Graphics)
                count += 1

                ' Se ci sono più linee, aggiungi una nuova pagina
                If count >= linesPerPage Then
                    e.HasMorePages = True
                    Return
                End If
            End While
        Next

        ' Verifica se ci sono più pagine
        e.HasMorePages = False
    End Sub






    'firebase-------------------------------------------------------------------



    Private Sub ShowErrorWithCopyOption(message As String)
        Dim result As DialogResult = MessageBox.Show(message & vbCrLf & vbCrLf & "Copia negli appunti?", "Errore", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
        If result = DialogResult.OK Then
            Clipboard.SetText(message)
        End If
    End Sub

    Private Function LoadFirebaseCredentials(filePath As String) As FirebaseCredentials
        If Not File.Exists(filePath) Then
            Return Nothing
        End If

        Dim credentials As New FirebaseCredentials

        Using reader As New StreamReader(filePath)
            credentials.Email = reader.ReadLine()?.Split("="c)(1)
            credentials.Password = reader.ReadLine()?.Split("="c)(1)
            credentials.ProjectId = GetProjectIdFromJson(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))
        End Using

        Return credentials
    End Function

    Private Async Function VerificaStatoFirebase(projectId As String) As Task
        Try
            Console.WriteLine("VerificaStatoFirebase - projectId: " & projectId)
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))
            Console.WriteLine("Variabile d'ambiente impostata")

            ' Testa la connessione a Firestore
            Dim db = FirestoreDb.Create(projectId)
            Console.WriteLine("FirestoreDb collegato con successo")

            ' Verifica la connessione facendo una semplice operazione, come una lettura della collezione
            Dim testQuery = db.Collection("notes").Limit(1)
            Dim testSnapshot = Await testQuery.GetSnapshotAsync()
            Console.WriteLine("Numero di documenti trovati nella collezione 'notes': " & testSnapshot.Documents.Count)

            If testSnapshot.Documents.Count > 0 Then
                Console.WriteLine("Connessione a Firebase riuscita, caricamento delle note da Firestore.")
                Await CaricaVociDaFirestore(projectId)
                lblStatus.Text = "Online"
                lblStatus.ForeColor = Color.Green

                ' Sincronizza le note salvate in cache (se esistono)
                Await SincronizzaNoteConFirebase()

            Else
                Console.WriteLine("La collezione 'notes' è vuota o non esiste.")
                Throw New Exception("La collezione 'notes' è vuota o non esiste.")
            End If

        Catch ex As Exception
            lblStatus.Text = "Offline"
            lblStatus.ForeColor = Color.Red
            Console.WriteLine("Errore durante la verifica dello stato di Firebase: " & ex.Message)

            ' Salva le note nella cache locale
            Console.WriteLine("Caricamento delle note dalla cache locale.")
            Dim noteInCache = CaricaNoteDaCache()
            If noteInCache IsNot Nothing AndAlso noteInCache.Count > 0 Then
                Console.WriteLine("Numero di note trovate nella cache: " & noteInCache.Count)
                lstFirebase.Items.Clear()
                For Each nota In noteInCache
                    Dim titolo = nota("Title").ToString()
                    Dim dataFirebase = DateTime.Parse(nota("DataFirebase").ToString())
                    lstFirebase.Items.Add($"{dataFirebase.ToShortDateString()}: {titolo}")
                Next
            Else
                Console.WriteLine("Nessuna nota trovata nella cache.")
            End If
        End Try
    End Function

    Private Sub OpenConsole()
        AllocConsole()
        Dim writer As New StreamWriter(Console.OpenStandardOutput())
        writer.AutoFlush = True
        Console.SetOut(writer)
    End Sub

    <Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function AllocConsole() As Boolean
    End Function


    Private Function GetProjectIdFromJson(filePath As String) As String
        If Not File.Exists(filePath) Then
            Return Nothing
        End If

        Try
            Dim json = File.ReadAllText(filePath)
            Dim jsonObject = JObject.Parse(json)
            Return jsonObject("project_id")?.ToString()
        Catch ex As Exception
            MessageBox.Show("Errore nella lettura del file JSON: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Private Sub btnLoginForm_Click(sender As Object, e As EventArgs) Handles btnLoginForm.Click
        Dim loginForm As New LoginForm
        If loginForm.ShowDialog = DialogResult.OK Then
            ' Verifica che il percorso del file sia valido
            If Not String.IsNullOrWhiteSpace(loginForm.FirebaseConfigFilePath) Then
                SaveFirebaseConfiguration(loginForm.FirebaseConfigFilePath)
            Else
                MessageBox.Show("Il file di configurazione è mancante.")
            End If
        End If
    End Sub


    Private Sub SaveFirebaseConfiguration(firebaseConfigFilePath As String)
        ' Definisce il percorso della cartella dell'applicazione
        Dim appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary")
        Directory.CreateDirectory(appDataPath)

        ' Salva il file JSON nella cartella dell'app
        File.Copy(firebaseConfigFilePath, Path.Combine(appDataPath, "firebaseconfig.json"), True)
    End Sub
    ' Classe per memorizzare le credenziali
    Private Class FirebaseCredentials
        Public Property Email As String
        Public Property Password As String
        Public Property ProjectId As String
    End Class

    Private Async Function AggiungiVoceSuFirestore(data As DateTime, titolo As String, nota As String, projectId As String) As Task
        Try
            Console.WriteLine("Inizio aggiunta della voce.")

            Dim db = FirestoreDb.Create(projectId)
            Console.WriteLine($"Connessione a Firestore creata con Project ID: {projectId}")

            ' Usa la data selezionata invece di quella corrente
            Dim dataUtc = New DateTime(data.Year, data.Month, data.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime()

            Console.WriteLine($"Data UTC selezionata: {dataUtc}")

            ' Recupera i valori di PCName e Flag
            Dim pcName = Environment.MachineName ' Nome del PC corrente
            Dim flag = txtFlag.Text ' Valore della TextBox per il flag

            ' Crea un documento con i dettagli della voce
            Dim docRef = db.Collection("notes").Document()
            Console.WriteLine($"Nuovo documento creato con ID: {docRef.Id}")

            Dim voce = New Dictionary(Of String, Object) From {
            {"DataFirebase", dataUtc},
            {"Title", titolo},
            {"Content", nota},
            {"PCName", pcName},
            {"Label", flag}
        }
            Console.WriteLine($"Dizionario voce: {String.Join(", ", voce.Select(Function(kv) $"{kv.Key}: {kv.Value}"))}")

            ' Aggiungi il documento a Firestore
            Await docRef.SetAsync(voce)
            Console.WriteLine("Voce aggiunta con successo a Firestore!")
            MessageBox.Show("Voce aggiunta con successo a Firestore!")

        Catch ex As Exception
            Console.WriteLine("Errore durante l'aggiunta della voce a Firestore: " & ex.Message)
            MessageBox.Show("Errore durante l'aggiunta della voce a Firestore: " & ex.Message)
        End Try
    End Function

    Private voceDictionary As New Dictionary(Of String, VoceFirebase)


    Private Async Function CaricaVociDaFirestore(projectId As String) As Task
        Try
            Console.WriteLine($"Progetto in uso: {projectId}")
            Dim db = FirestoreDb.Create(projectId)
            Console.WriteLine("FirestoreDb trovato con successo.")

            Dim query = db.Collection("notes").OrderBy("DataFirebase")
            Console.WriteLine("Query creata. Inizio recupero documenti.")

            Dim querySnapshot = Await query.GetSnapshotAsync()
            Console.WriteLine($"Documenti recuperati: {querySnapshot.Documents.Count}")

            lstFirebase.Items.Clear()
            voceDictionary.Clear()

            For Each document In querySnapshot.Documents
                Dim data = document.GetValue(Of DateTime)("DataFirebase").ToLocalTime()
                Dim titolo = document.GetValue(Of String)("Title")
                Dim contenuto = document.GetValue(Of String)("Content")
                Dim pcname = document.GetValue(Of String)("PCName")
                Dim flag = document.GetValue(Of String)("Label")
                Dim voce = New VoceFirebase(data, titolo, contenuto, pcname, flag)

                ' Usa l'ID del documento come chiave unica
                Dim key = document.Id
                If Not voceDictionary.ContainsKey(key) Then
                    voceDictionary.Add(key, voce)
                    lstFirebase.Items.Add($"{data.ToShortDateString()}: {titolo}")
                    Console.WriteLine($"Voce aggiunta: {data.ToShortDateString()}: {titolo}")
                End If
            Next
            AggiornaCalendario() ' dovrebbe mostrare anche le voci sul calendario di firebase
        Catch ex As Exception
            Console.WriteLine("Errore durante il caricamento delle voci: " & ex.Message)
            MessageBox.Show("Errore durante il caricamento delle voci: " & ex.Message)
        End Try
    End Function

    Private Sub lstFirebase_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstFirebase.SelectedIndexChanged
        ' Evita l'aggiornamento del calendario se siamo già in fase di aggiornamento
        If updatingCalendar Then Return


        ' Verifica se una voce è selezionata
        If lstFirebase.SelectedIndex >= 0 Then
            Dim selectedItem = DirectCast(lstFirebase.SelectedItem, String)

            ' Trova la chiave corrispondente
            Dim voceSelezionata As VoceFirebase = Nothing
            For Each key In voceDictionary.Keys
                Dim voce = voceDictionary(key)
                If $"{voce.DataFirebase.ToShortDateString()}: {voce.Title}" = selectedItem Then
                    voceSelezionata = voce
                    Exit For
                End If
            Next

            If voceSelezionata IsNot Nothing Then
                ' Mostra i dettagli della voce selezionata
                txtTitolo.Text = voceSelezionata.Title
                dtpData.Value = voceSelezionata.DataFirebase
                lblDataNota.Text = voceSelezionata.DataFirebase
                lblPCName.Text = voceSelezionata.PCName
                txtFlag.Text = voceSelezionata.Flag
                'cmbEmozione.SelectedItem = voceSelezionata.Emotion
                rtbContenuto.Text = voceSelezionata.Content

                ' Evita loop impostando il flag
                updatingCalendar = True
                ' Seleziona la data nel calendario
                MonthCalendar1.SelectionStart = voceSelezionata.DataFirebase
                MonthCalendar1.SelectionEnd = voceSelezionata.DataFirebase
                ' Ripristina il flag
                updatingCalendar = False
            End If
        Else


        End If
    End Sub




    Private Async Function EliminaVoceSuFirestore(documentId As String, projectId As String) As Task
        Try
            Dim db = FirestoreDb.Create(projectId)
            Dim docRef = db.Collection("notes").Document(documentId)
            Await docRef.DeleteAsync()
            Console.WriteLine("Voce eliminata con id" & documentId)
            MessageBox.Show("Voce eliminata con successo da Firestore!")
        Catch ex As Exception
            Console.WriteLine("Errore durante l'eliminazione della voce da Firestore: " & ex.Message And documentId)
            MessageBox.Show("Errore durante l'eliminazione della voce da Firestore: " & ex.Message)
        End Try
    End Function

    Private Async Function ModificaVoceSuFirestore(documentId As String, nuovaData As DateTime, nuovoTitolo As String, nuovoContenuto As String, projectId As String) As Task
        Try
            Console.WriteLine($"Inizio modifica della voce: {documentId}")

            Dim db = FirestoreDb.Create(projectId)
            Console.WriteLine($"Connessione a Firestore creata con Project ID: {projectId}")

            Dim docRef = db.Collection("notes").Document(documentId)
            Console.WriteLine($"Referenza documento: {docRef.Id}")

            ' Usa la data selezionata dal DateTimePicker
            Dim dataSelezionata As Date = dtpData.Value
            Console.WriteLine($"Data selezionata dal DateTimePicker: {dataSelezionata}")

            ' Converti la data e l'ora in UTC
            Dim dataUtc As DateTime = dataSelezionata.ToUniversalTime()
            Console.WriteLine($"Data convertita in UTC: {dataUtc}")

            ' Recupera i valori di PCName e Flag
            Dim pcName = Environment.MachineName ' Nome del PC corrente
            Dim flag = txtFlag.Text ' Valore della TextBox per il flag

            ' Crea il dizionario degli aggiornamenti
            Dim aggiornamenti = New Dictionary(Of String, Object) From {
            {"DataFirebase", dataUtc},
            {"Title", nuovoTitolo},
            {"Content", nuovoContenuto},
            {"PCName", pcName},
            {"Label", flag}
        }
            Console.WriteLine($"Dizionario aggiornamenti: {String.Join(", ", aggiornamenti.Select(Function(kv) $"{kv.Key}: {kv.Value}"))}")

            ' Aggiorna il documento in Firestore
            Await docRef.SetAsync(aggiornamenti, SetOptions.MergeAll)
            Console.WriteLine("Voce modificata con successo!")
            MessageBox.Show("Voce modificata con successo!")

        Catch ex As Exception
            Console.WriteLine("Errore durante la modifica della voce: " & ex.Message)
            MessageBox.Show("Errore durante la modifica della voce: " & ex.Message)
        End Try
    End Function


    'krypton calendar esperimento-----------------------------

    ' Lista delle date con note dal database locale
    Private dateConNote As List(Of Date) = New List(Of Date)()
    ' Lista delle date con note dal database Firebase
    Private dateConNoteFirebase As List(Of Date) = New List(Of Date)()

    Private Sub AggiornaCalendario()
        ' Recupera le date con note dal database Firebase
        dateConNoteFirebase = RecuperaDateConNoteFirebase()
        Console.WriteLine("Date dal database Firebase:")
        For Each d In dateConNoteFirebase
            Console.WriteLine(d.ToString("yyyy/MM/dd HH:mm:ss"))
        Next

        ' Normalizza le date (rimuovere ore, minuti e secondi)
        Dim normalizeDate = Function(d As DateTime) As DateTime
                                Return d.Date ' Converte a solo data
                            End Function

        ' Accorpa le date, rimuovendo i duplicati e normalizzandole
        Dim tutteLeDate As HashSet(Of DateTime) = New HashSet(Of DateTime)(
      dateConNote.Concat(dateConNoteFirebase).Select(Function(d) normalizeDate(d))
  )
        Console.WriteLine("Date accorpate senza duplicati:")
        For Each d In tutteLeDate
            Console.WriteLine(d.ToString("yyyy/MM/dd HH:mm:ss"))
        Next

        ' Imposta le date con note come date evidenziate nel KryptonMonthCalendar
        MonthCalendar1.BoldedDates = tutteLeDate.ToArray()

        ' Debug: Verifica le date impostate nel calendario
        Console.WriteLine("Date evidenziate nel KryptonMonthCalendar:")
        For Each d In MonthCalendar1.BoldedDates
            Console.WriteLine(d.ToString("yyyy/MM/dd HH:mm:ss"))
        Next

        ' Invalida il controllo per forzare il ridisegno
        MonthCalendar1.Invalidate()
    End Sub

    Private Sub MonthCalendar1_DateChanged(sender As Object, e As DateRangeEventArgs) Handles MonthCalendar1.DateChanged

        ' Gestisci la logica quando la data del calendario cambia
        If updatingCalendar Then Return ' Evita loop
        ' Aggiorna la lista delle voci del diario quando la data del MonthCalendar cambia
        '  AggiornaListaNota(MonthCalendar1.SelectionStart)

        ' Seleziona le date corrispondenti nella lstFirebase
        SelezionaDateFirebase(MonthCalendar1.SelectionStart)
    End Sub


    ' Flag per evitare loop
    Private updatingCalendar As Boolean = False
    Private Sub KryptonMonthCalendar1_DateChanged(sender As Object, e As DateRangeEventArgs)

        ' Gestisci la logica quando la data del calendario cambia
        If updatingCalendar Then Return ' Evita loop
        ' Aggiorna la lista delle voci del diario quando la data del MonthCalendar cambia


        ' Seleziona le date corrispondenti nella lstFirebase
        SelezionaDateFirebase(e.Start)
    End Sub

    Private Sub SelezionaDateFirebase(dataSelezionata As DateTime)
        ' Deselect all items first
        lstFirebase.ClearSelected()

        ' Normalize the selected date
        Dim dataNorm = dataSelezionata.Date
        Console.WriteLine($"Data selezionata: {dataNorm.ToString("yyyy/MM/dd")}")

        ' Select items in lstFirebase that match the selected date
        For i As Integer = 0 To lstFirebase.Items.Count - 1
            Dim itemText As String = lstFirebase.Items(i).ToString()
            Dim itemDate As DateTime

            ' Try to extract and parse the date from the item text
            Try
                ' Assuming the date is at the beginning followed by ": "
                Dim datePart As String = itemText.Split(":"c)(0).Trim()
                itemDate = DateTime.Parse(datePart)

                ' Check if the normalized date matches
                If itemDate.Date = dataNorm Then
                    lstFirebase.SetSelected(i, True)
                    Console.WriteLine($"Elemento selezionato: {itemText}")
                End If
            Catch ex As FormatException
                ' Handle cases where the date is not in the expected format
                Console.WriteLine($"Impossibile analizzare la data dall'elemento: {itemText}")
            End Try
        Next
    End Sub



    ' Funzione di esempio per recuperare le date con note dal database Firebase
    Private Function RecuperaDateConNoteFirebase() As List(Of DateTime)
        ' Implementa la logica per recuperare le date dal database Firebase
        Return voceDictionary.Values.Select(Function(v) v.DataFirebase).ToList()
    End Function


    'fine esperimento krypton-------------------------------
    'cache----------------------------------------------------------------------------------------------------
    Private Sub SalvaNoteInCache(voce As Dictionary(Of String, Object))
        Try
            ' Percorso del file JSON per la cache locale
            Dim cacheFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "noteCache.json")

            ' Leggi la cache esistente
            Dim note As New List(Of Dictionary(Of String, Object))
            If File.Exists(cacheFilePath) Then
                Dim json As String = File.ReadAllText(cacheFilePath)
                note = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json)
            End If

            ' Aggiungi la nuova voce alla lista
            note.Add(voce)

            ' Salva la lista aggiornata nel file JSON
            Dim updatedJson As String = JsonConvert.SerializeObject(note, Formatting.Indented)
            File.WriteAllText(cacheFilePath, updatedJson)
        Catch ex As Exception
            MessageBox.Show("Errore durante il salvataggio della voce in cache: " & ex.Message)
        End Try
    End Sub

    Private Function CaricaNoteDaCache() As List(Of Dictionary(Of String, Object))
        Dim note As New List(Of Dictionary(Of String, Object))()

        Try
            ' Percorso del file JSON per la cache locale
            Dim cacheFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "noteCache.json")

            ' Verifica se il file esiste
            If File.Exists(cacheFilePath) Then
                ' Leggi il contenuto del file JSON
                Dim json As String = File.ReadAllText(cacheFilePath)

                ' Deserializza il JSON in una lista di note
                note = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json)
            End If
        Catch ex As Exception
            MessageBox.Show("Errore durante il caricamento delle note dalla cache: " & ex.Message)
        End Try

        Return note
    End Function


    Private Async Function SincronizzaNoteConFirebase() As Task
        Try
            ' Carica le note dalla cache
            Dim noteInCache = CaricaNoteDaCache()

            If noteInCache IsNot Nothing AndAlso noteInCache.Count > 0 Then
                ' Connessione a Firestore
                Dim projectId = GetProjectIdFromJson(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))
                Dim db = FirestoreDb.Create(projectId)

                ' Carica ogni nota su Firebase
                For Each nota In noteInCache
                    Dim newDocRef = db.Collection("notes").Document()
                    Await newDocRef.SetAsync(nota)
                Next

                ' Mostra un messaggio di aggiornamento
                MessageBox.Show("Le note salvate in cache sono state caricate su Firebase.")
                AggiornaCalendario()
                Await CaricaVociDaFirestore(projectId)

                ' Cancella il file JSON della cache
                Dim cacheFilePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "noteCache.json")
                If File.Exists(cacheFilePath) Then
                    File.Delete(cacheFilePath)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Errore durante la sincronizzazione delle note con Firebase: " & ex.Message)
        End Try
    End Function

    'cache---------------------------------------------------------------------------------------------
    'ricerca inizio -----------------------------------------------------------------------------------------

    ' Gestore dell'evento TextChanged per la ComboBox
    Private Async Sub cmbKeyword_TextChanged(sender As Object, e As EventArgs) Handles cmbKeyword.TextChanged
        Dim keyword As String = cmbKeyword.Text.Trim()

        ' Verifica che l'utente abbia digitato almeno tre lettere
        If keyword.Length > 1 Then
            btnClear.Visible = keyword.Length > 0 ' Nasconde il btnClear quando l'ultima lettera viene cancellata
            lstFirebase.Items.Clear() ' Pulisce la ListBox se la keyword è troppo corta
            Await CaricaVociDaFirestore(projectId) ' Ritorna alla visualizzazione normale
            Return
        End If

        ' Mostra il btnClear se ci sono almeno 3 caratteri
        btnClear.Visible = True

        ' Filtra le note in base alla keyword
        Await FiltraNote(keyword)
    End Sub


    ' Funzione per filtrare le note su Firestore in base alla keyword
    Private Async Function FiltraNote(keyword As String) As Task
        Try
            ' Ottieni il Project ID dal file JSON di configurazione
            Dim projectId = GetProjectIdFromJson(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary", "firebaseconfig.json"))

            If String.IsNullOrEmpty(projectId) Then
                MessageBox.Show("Impossibile ottenere il Project ID dal file JSON di configurazione.")
                Return
            End If

            ' Connessione a Firestore
            Dim db = FirestoreDb.Create(projectId)

            ' Ottieni tutti i documenti dalla raccolta "notes"
            Dim query = db.Collection("notes")
            Dim snapshot = Await query.GetSnapshotAsync()

            ' Debug: Controlla il numero di documenti trovati
            Console.WriteLine($"Documenti trovati: {snapshot.Count}")

            ' Pulisci la ListBox prima di aggiungere i nuovi risultati
            lstFirebase.Items.Clear()

            ' Filtra i documenti in base alla keyword
            Dim normalizedKeyword = keyword.ToLower().Trim()
            For Each doc In snapshot.Documents
                Dim titolo = doc.GetValue(Of String)("Title").ToLower()
                Dim contenuto = doc.GetValue(Of String)("Content").ToLower()

                ' Verifica se la keyword è presente nel titolo o nel contenuto
                If titolo.Contains(normalizedKeyword) OrElse contenuto.Contains(normalizedKeyword) Then
                    Dim dataFirebase = doc.GetValue(Of DateTime)("DataFirebase")
                    lstFirebase.Items.Add($"{dataFirebase.ToShortDateString()}: {doc.GetValue(Of String)("Title")}")
                End If
            Next

            If lstFirebase.Items.Count = 0 Then
                lstFirebase.Items.Add("Nessun risultato trovato.")
            End If

        Catch ex As Exception
            Console.WriteLine("Errore durante la ricerca delle note: " & ex.Message)
            MessageBox.Show("Errore durante la ricerca delle note: " & ex.Message)
        End Try
    End Function

    ' Aggiungi un Button alla tua form, posizionalo accanto alla ComboBox e chiamalo btnClear

    Private Async Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' Svuota la ComboBox e ripristina la ricerca
        cmbKeyword.Text = String.Empty
        lstFirebase.Items.Clear() ' Se vuoi pulire anche la ListBox
        ' Eventualmente richiama una funzione per ripristinare lo stato iniziale
        AggiornaCalendario()
        Await CaricaVociDaFirestore(projectId)
    End Sub

    Private Sub btnRelease_Click(sender As Object, e As EventArgs) Handles btnRelease.Click
        SplashScreen1.ShowDialog()
    End Sub



    'ricerca fine---------------------------------------------------------------------------------------------



    'firebase --------------------------------------------------------------------

End Class
'classe firebase usata per gestire remoto
<FirestoreData>
Public Class VoceFirebase
    <FirestoreProperty>
    Public Property DataFirebase As DateTime
        Get
            Return _dataFirebase
        End Get
        Set(value As DateTime)
            _dataFirebase = DateTime.SpecifyKind(value, DateTimeKind.Utc)
        End Set
    End Property
    Private _dataFirebase As DateTime

    <FirestoreProperty>
    Public Property Title As String

    <FirestoreProperty>
    Public Property Content As String

    <FirestoreProperty>
    Public Property PCName As String

    <FirestoreProperty>
    Public Property Flag As String

    ' Costruttore senza parametri richiesto per Firestore
    Public Sub New()
    End Sub

    ' Costruttore con parametri
    Public Sub New(dataFirebase As DateTime, titolo As String, contenuto As String, pcname As String, flag As String)
        Me.DataFirebase = DateTime.SpecifyKind(dataFirebase, DateTimeKind.Utc)
        Me.Title = titolo
        Me.Content = contenuto
        Me.PCName = pcname
        Me.Flag = flag
    End Sub

    Public Overrides Function ToString() As String
        Return $"{DataFirebase.ToString("dd/MM/yyyy")} - {Title}"
    End Function
End Class

Public Class Nota
    Public Property Id As String
    Public Property Title As String
    Public Property Content As String
    Public Property DataFirebase As DateTime
End Class




Public Class SpellChecker

    Private ReadOnly _dictionary As WordList
    Private ReadOnly _userDictionaryPath As String = "userDictionary.txt"
    Private _customWords As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    Public Sub New()
        ' Caricamento del dizionario principale
        _dictionary = WordList.CreateFromFiles("it_IT.dic")

        ' Caricamento delle parole personalizzate
        If File.Exists(_userDictionaryPath) Then
            For Each word In File.ReadAllLines(_userDictionaryPath)
                _customWords.Add(word.Trim())
            Next
        End If
    End Sub

    Public Function CheckSpelling(text As String) As List(Of String)
        Dim misspelledWords As New List(Of String)()

        ' Modifica questa riga per gestire le parole con apostrofi come un'unità singola
        Dim wordsToCheck = Regex.Matches(text, "\b\w+('\w+)?\b").Cast(Of Match)().Select(Function(m) m.Value).ToArray()

        For Each word In wordsToCheck
            word = word.Trim("?".ToCharArray())
            If Not _dictionary.Check(word) AndAlso Not _customWords.Contains(word) Then
                misspelledWords.Add(word)
            End If
        Next

        Return misspelledWords
    End Function
    Public Sub AddToUserDictionary(word As String)
        If Not _customWords.Contains(word) Then
            _customWords.Add(word)
            File.AppendAllText(_userDictionaryPath, word + Environment.NewLine)
        End If
    End Sub

    Public Function CheckSingleWord(word As String) As Boolean
        Return _dictionary.Check(word) OrElse _customWords.Contains(word)
    End Function

    Public Function GetSuggestions(word As String) As List(Of String)
        Dim suggestions As New List(Of String)

        ' Suggerimenti dal dizionario principale
        suggestions.AddRange(_dictionary.Suggest(word))

        ' Suggerimenti dal dizionario personalizzato
        Dim customSuggestions = _customWords.Where(Function(w) LevenshteinDistance.Compute(w, word) <= 2).OrderBy(Function(w) LevenshteinDistance.Compute(w, word)).ToList()
        suggestions.AddRange(customSuggestions)

        Return suggestions.Distinct().ToList()
    End Function
End Class
Public Class LevenshteinDistance
    Public Shared Function Compute(s As String, t As String) As Integer
        If s Is Nothing Then
            Throw New ArgumentNullException("s")
        End If

        If t Is Nothing Then
            Throw New ArgumentNullException("t")
        End If

        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim d As Integer(,) = New Integer(n, m) {}

        If n = 0 Then
            Return m
        End If

        If m = 0 Then
            Return n
        End If

        For i As Integer = 0 To n
            d(i, 0) = i
        Next

        For j As Integer = 0 To m
            d(0, j) = j
        Next

        For i As Integer = 1 To n
            For j As Integer = 1 To m
                Dim cost As Integer = (If(t(j - 1) = s(i - 1), 0, 1))
                d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + cost)
            Next
        Next

        Return d(n, m)
    End Function

End Class

