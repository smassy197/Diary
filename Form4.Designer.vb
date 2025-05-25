<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form4))
        btnBackup = New Button()
        btnApri = New Button()
        btnCarica = New Button()
        lblBackupFileName = New Label()
        SuspendLayout()
        ' 
        ' btnBackup
        ' 
        btnBackup.Location = New Point(215, 127)
        btnBackup.Name = "btnBackup"
        btnBackup.Size = New Size(94, 29)
        btnBackup.TabIndex = 0
        btnBackup.Text = "Backup"
        btnBackup.UseVisualStyleBackColor = True
        ' 
        ' btnApri
        ' 
        btnApri.Location = New Point(215, 208)
        btnApri.Name = "btnApri"
        btnApri.Size = New Size(94, 29)
        btnApri.TabIndex = 1
        btnApri.Text = "Apri"
        btnApri.UseVisualStyleBackColor = True
        ' 
        ' btnCarica
        ' 
        btnCarica.Location = New Point(346, 208)
        btnCarica.Name = "btnCarica"
        btnCarica.Size = New Size(94, 29)
        btnCarica.TabIndex = 2
        btnCarica.Text = "Carica"
        btnCarica.UseVisualStyleBackColor = True
        ' 
        ' lblBackupFileName
        ' 
        lblBackupFileName.AutoSize = True
        lblBackupFileName.Location = New Point(215, 277)
        lblBackupFileName.Name = "lblBackupFileName"
        lblBackupFileName.Size = New Size(101, 20)
        lblBackupFileName.TabIndex = 3
        lblBackupFileName.Text = "File di backup"
        ' 
        ' Form4
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(lblBackupFileName)
        Controls.Add(btnCarica)
        Controls.Add(btnApri)
        Controls.Add(btnBackup)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Form4"
        Text = "Backup"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnBackup As Button
    Friend WithEvents btnApri As Button
    Friend WithEvents btnCarica As Button
    Friend WithEvents lblBackupFileName As Label
End Class
