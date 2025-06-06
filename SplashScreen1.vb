﻿Public NotInheritable Class SplashScreen1

    'TODO: questo form può essere facilmente impostato come schermata iniziale per l'applicazione dalla scheda "Applicazione"
    '  di Creazione progetti (scegliere "Proprietà" dal menu "Progetto").


    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Imposta il testo della finestra di dialogo in fase di esecuzione in base alle informazioni sull'assembly dell'applicazione.  

        'TODO: personalizzare le informazioni sull'assembly dell'applicazione nel riquadro "Applicazione" 
        '  della finestra delle proprietà del progetto (accessibile dal menu "Progetto").

        'Titolo dell'applicazione
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            'Se il titolo dell'applicazione manca, verrà usato il nome dell'applicazione, senza l'estensione
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        'Formatta le informazioni sulla versione utilizzando come stringa di formattazione il testo impostato nel controllo Version in fase di esecuzione.
        '  Consente una localizzazione efficace, se necessario.
        '  Le informazioni sulla build e la revisione possono essere incluse utilizzando il codice seguente e modificando 
        '  in "Versione {0}.{1:00}.{2}.{3}", o simile, il testo del controllo Version impostato in fase di progettazione.
        '  Per ulteriori informazioni, vedere String.Format() nella Guida.
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)

        'Informazioni sul copyright
        Copyright.Text = My.Application.Info.Copyright
    End Sub

    Private Sub MainLayoutPanel_Paint(sender As Object, e As PaintEventArgs) Handles MainLayoutPanel.Paint

    End Sub

    Private Sub btnChiudi_Click(sender As Object, e As EventArgs) Handles btnChiudi.Click
        Me.Close()
    End Sub
End Class
