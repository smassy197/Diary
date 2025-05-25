<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginForm
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        txtFirebaseConfigFilePath = New TextBox()
        btnLogin = New Button()
        Label1 = New Label()
        btnSelectConfigFile = New Button()
        SuspendLayout()
        ' 
        ' txtFirebaseConfigFilePath
        ' 
        txtFirebaseConfigFilePath.Location = New Point(66, 63)
        txtFirebaseConfigFilePath.Name = "txtFirebaseConfigFilePath"
        txtFirebaseConfigFilePath.Size = New Size(286, 27)
        txtFirebaseConfigFilePath.TabIndex = 0
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(659, 63)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(94, 29)
        btnLogin.TabIndex = 9
        btnLogin.Text = "Login"
        btnLogin.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(412, 68)
        Label1.Name = "Label1"
        Label1.Size = New Size(63, 20)
        Label1.TabIndex = 10
        Label1.Text = "File json"
        ' 
        ' btnSelectConfigFile
        ' 
        btnSelectConfigFile.Location = New Point(12, 62)
        btnSelectConfigFile.Name = "btnSelectConfigFile"
        btnSelectConfigFile.Size = New Size(48, 29)
        btnSelectConfigFile.TabIndex = 15
        btnSelectConfigFile.Text = "Apri"
        btnSelectConfigFile.UseVisualStyleBackColor = True
        btnSelectConfigFile.UseWaitCursor = True
        ' 
        ' LoginForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 184)
        Controls.Add(btnSelectConfigFile)
        Controls.Add(Label1)
        Controls.Add(btnLogin)
        Controls.Add(txtFirebaseConfigFilePath)
        Name = "LoginForm"
        Text = "Login"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtFirebaseConfigFilePath As TextBox
    Friend WithEvents btnLogin As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSelectConfigFile As Button
End Class
