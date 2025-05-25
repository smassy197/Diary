Imports System.IO

Public Class LoginForm
    Public Property FirebaseConfigFilePath As String

    Private Sub btnSelectConfigFile_Click(sender As Object, e As EventArgs) Handles btnSelectConfigFile.Click
        ' Crea e configura l'OpenFileDialog
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Title = "Seleziona il file di configurazione Firebase"
        openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"

        ' Mostra la finestra di dialogo e controlla il risultato
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            txtFirebaseConfigFilePath.Text = openFileDialog.FileName
            FirebaseConfigFilePath = openFileDialog.FileName
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Imposta la proprietà con il valore dell'input
        FirebaseConfigFilePath = txtFirebaseConfigFilePath.Text

        ' Verifica se il percorso del file JSON di configurazione è valido
        If String.IsNullOrWhiteSpace(FirebaseConfigFilePath) OrElse Not File.Exists(FirebaseConfigFilePath) Then
            MessageBox.Show("Il percorso del file di configurazione Firebase non è valido.")
            Return
        End If

        ' Ottieni la cartella dell'applicazione
        Dim appDataFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Diary")

        ' Assicurati che la cartella esista
        If Not Directory.Exists(appDataFolder) Then
            Directory.CreateDirectory(appDataFolder)
        End If

        ' Percorso completo per il file di configurazione JSON
        Dim jsonFilePath As String = Path.Combine(appDataFolder, "firebaseconfig.json")

        ' Copia il file JSON nella cartella dell'app
        Try
            File.Copy(FirebaseConfigFilePath, jsonFilePath, True)
        Catch ex As Exception
            MessageBox.Show("Errore nella copia del file di configurazione JSON: " & ex.Message)
            Return
        End Try

        ' Mostra un messaggio che informa di riavviare l'applicazione
        MessageBox.Show("Configurazione completata. Per applicare le modifiche, è necessario riavviare l'applicazione.", "Riavviare l'applicazione", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Chiudi tutto
        Application.Exit()
    End Sub

End Class




