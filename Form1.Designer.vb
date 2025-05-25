<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        txtTitolo = New TextBox()
        rtbContenuto = New RichTextBox()
        btnReset = New Button()
        btnCheckSpelling = New Button()
        PictureBox1 = New PictureBox()
        btnIngrandisci = New Button()
        btnDiminuisci = New Button()
        btnStampa = New Button()
        btnLoginForm = New Button()
        lstFirebase = New ListBox()
        btnAggiungiVoceFirestore = New Button()
        btnEliminaVoceFirestore = New Button()
        dtpData = New DateTimePicker()
        btnMostraForm4 = New Krypton.Toolkit.KryptonButton()
        connectionCheckTimer = New Timer(components)
        lblStatus = New Label()
        lblDataNota = New Label()
        MonthCalendar1 = New MonthCalendar()
        cmbKeyword = New ComboBox()
        btnClear = New Krypton.Toolkit.KryptonButton()
        lblPCName = New Label()
        txtFlag = New TextBox()
        Label1 = New Label()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtTitolo
        ' 
        txtTitolo.Font = New Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtTitolo.Location = New Point(12, 296)
        txtTitolo.Name = "txtTitolo"
        txtTitolo.Size = New Size(961, 31)
        txtTitolo.TabIndex = 2
        ' 
        ' rtbContenuto
        ' 
        rtbContenuto.Font = New Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        rtbContenuto.Location = New Point(12, 333)
        rtbContenuto.Name = "rtbContenuto"
        rtbContenuto.Size = New Size(1162, 289)
        rtbContenuto.TabIndex = 3
        rtbContenuto.Text = ""
        ' 
        ' btnReset
        ' 
        btnReset.Location = New Point(1180, 280)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(117, 47)
        btnReset.TabIndex = 8
        btnReset.Text = "Reset"
        btnReset.UseVisualStyleBackColor = True
        ' 
        ' btnCheckSpelling
        ' 
        btnCheckSpelling.Location = New Point(1180, 227)
        btnCheckSpelling.Name = "btnCheckSpelling"
        btnCheckSpelling.Size = New Size(117, 47)
        btnCheckSpelling.TabIndex = 9
        btnCheckSpelling.Text = "Correzzione"
        btnCheckSpelling.UseVisualStyleBackColor = True
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(1188, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(109, 113)
        PictureBox1.TabIndex = 10
        PictureBox1.TabStop = False
        ' 
        ' btnIngrandisci
        ' 
        btnIngrandisci.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnIngrandisci.Location = New Point(311, 233)
        btnIngrandisci.Name = "btnIngrandisci"
        btnIngrandisci.Size = New Size(127, 57)
        btnIngrandisci.TabIndex = 11
        btnIngrandisci.Text = "+"
        btnIngrandisci.UseVisualStyleBackColor = True
        ' 
        ' btnDiminuisci
        ' 
        btnDiminuisci.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        btnDiminuisci.Location = New Point(455, 233)
        btnDiminuisci.Name = "btnDiminuisci"
        btnDiminuisci.Size = New Size(126, 57)
        btnDiminuisci.TabIndex = 12
        btnDiminuisci.Text = "-"
        btnDiminuisci.UseVisualStyleBackColor = True
        ' 
        ' btnStampa
        ' 
        btnStampa.Location = New Point(1139, 177)
        btnStampa.Name = "btnStampa"
        btnStampa.Size = New Size(158, 48)
        btnStampa.TabIndex = 13
        btnStampa.Text = "Stampa"
        btnStampa.UseVisualStyleBackColor = True
        ' 
        ' btnLoginForm
        ' 
        btnLoginForm.Location = New Point(1180, 499)
        btnLoginForm.Name = "btnLoginForm"
        btnLoginForm.Size = New Size(117, 29)
        btnLoginForm.TabIndex = 21
        btnLoginForm.Text = "Login"
        btnLoginForm.UseVisualStyleBackColor = True
        ' 
        ' lstFirebase
        ' 
        lstFirebase.Font = New Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lstFirebase.FormattingEnabled = True
        lstFirebase.ItemHeight = 25
        lstFirebase.Location = New Point(293, 71)
        lstFirebase.Name = "lstFirebase"
        lstFirebase.Size = New Size(840, 154)
        lstFirebase.TabIndex = 22
        ' 
        ' btnAggiungiVoceFirestore
        ' 
        btnAggiungiVoceFirestore.Location = New Point(1180, 333)
        btnAggiungiVoceFirestore.Name = "btnAggiungiVoceFirestore"
        btnAggiungiVoceFirestore.Size = New Size(117, 42)
        btnAggiungiVoceFirestore.TabIndex = 23
        btnAggiungiVoceFirestore.Text = "Salva"
        btnAggiungiVoceFirestore.UseVisualStyleBackColor = True
        ' 
        ' btnEliminaVoceFirestore
        ' 
        btnEliminaVoceFirestore.Location = New Point(1180, 381)
        btnEliminaVoceFirestore.Name = "btnEliminaVoceFirestore"
        btnEliminaVoceFirestore.Size = New Size(117, 39)
        btnEliminaVoceFirestore.TabIndex = 24
        btnEliminaVoceFirestore.Text = "Elimina"
        btnEliminaVoceFirestore.UseVisualStyleBackColor = True
        ' 
        ' dtpData
        ' 
        dtpData.Font = New Font("Segoe UI", 10.8F)
        dtpData.Format = DateTimePickerFormat.Short
        dtpData.Location = New Point(998, 296)
        dtpData.Name = "dtpData"
        dtpData.Size = New Size(154, 31)
        dtpData.TabIndex = 19
        ' 
        ' btnMostraForm4
        ' 
        btnMostraForm4.Location = New Point(1184, 448)
        btnMostraForm4.Name = "btnMostraForm4"
        btnMostraForm4.Size = New Size(112, 31)
        btnMostraForm4.TabIndex = 26
        btnMostraForm4.Values.DropDownArrowColor = Color.Empty
        btnMostraForm4.Values.Text = "Manutenzione"
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatus.Location = New Point(601, 249)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(74, 28)
        lblStatus.TabIndex = 27
        lblStatus.Text = "Label1"
        ' 
        ' lblDataNota
        ' 
        lblDataNota.AutoSize = True
        lblDataNota.Location = New Point(12, 249)
        lblDataNota.Name = "lblDataNota"
        lblDataNota.Size = New Size(0, 20)
        lblDataNota.TabIndex = 28
        ' 
        ' MonthCalendar1
        ' 
        MonthCalendar1.Location = New Point(12, 21)
        MonthCalendar1.Name = "MonthCalendar1"
        MonthCalendar1.TabIndex = 29
        ' 
        ' cmbKeyword
        ' 
        cmbKeyword.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbKeyword.FormattingEnabled = True
        cmbKeyword.Location = New Point(293, 21)
        cmbKeyword.Name = "cmbKeyword"
        cmbKeyword.Size = New Size(288, 36)
        cmbKeyword.TabIndex = 30
        ' 
        ' btnClear
        ' 
        btnClear.Location = New Point(592, 29)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(16, 20)
        btnClear.TabIndex = 31
        btnClear.Values.DropDownArrowColor = Color.Empty
        btnClear.Values.Text = "x"
        ' 
        ' lblPCName
        ' 
        lblPCName.AutoSize = True
        lblPCName.Location = New Point(806, 29)
        lblPCName.Name = "lblPCName"
        lblPCName.Size = New Size(0, 20)
        lblPCName.TabIndex = 32
        ' 
        ' txtFlag
        ' 
        txtFlag.Location = New Point(936, 242)
        txtFlag.Name = "txtFlag"
        txtFlag.Size = New Size(197, 27)
        txtFlag.TabIndex = 33
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(661, 29)
        Label1.Name = "Label1"
        Label1.Size = New Size(129, 20)
        Label1.TabIndex = 34
        Label1.Text = "Nome dispositivo:"
        ' 
        ' Form1
        ' 
        AcceptButton = btnDiminuisci
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1309, 634)
        Controls.Add(Label1)
        Controls.Add(txtFlag)
        Controls.Add(lblPCName)
        Controls.Add(btnClear)
        Controls.Add(cmbKeyword)
        Controls.Add(MonthCalendar1)
        Controls.Add(lblDataNota)
        Controls.Add(lblStatus)
        Controls.Add(btnMostraForm4)
        Controls.Add(btnEliminaVoceFirestore)
        Controls.Add(btnAggiungiVoceFirestore)
        Controls.Add(lstFirebase)
        Controls.Add(btnLoginForm)
        Controls.Add(dtpData)
        Controls.Add(btnStampa)
        Controls.Add(btnDiminuisci)
        Controls.Add(btnIngrandisci)
        Controls.Add(PictureBox1)
        Controls.Add(btnCheckSpelling)
        Controls.Add(btnReset)
        Controls.Add(rtbContenuto)
        Controls.Add(txtTitolo)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "Form1"
        Text = "Diario"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents txtTitolo As TextBox
    Friend WithEvents rtbContenuto As RichTextBox
    Friend WithEvents btnReset As Button
    Friend WithEvents btnCheckSpelling As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnIngrandisci As Button
    Friend WithEvents btnDiminuisci As Button
    Friend WithEvents btnStampa As Button
    Friend WithEvents btnLoginForm As Button
    Friend WithEvents lstFirebase As ListBox
    Friend WithEvents btnAggiungiVoceFirestore As Button
    Friend WithEvents btnEliminaVoceFirestore As Button
    Friend WithEvents dtpData As DateTimePicker
    Friend WithEvents btnMostraForm4 As Krypton.Toolkit.KryptonButton
    Friend WithEvents connectionCheckTimer As Timer
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblDataNota As Label
    Friend WithEvents MonthCalendar1 As MonthCalendar
    Friend WithEvents cmbKeyword As ComboBox
    Friend WithEvents btnClear As Krypton.Toolkit.KryptonButton
    Friend WithEvents lblPCName As Label
    Friend WithEvents txtFlag As TextBox
    Friend WithEvents Label1 As Label

End Class
