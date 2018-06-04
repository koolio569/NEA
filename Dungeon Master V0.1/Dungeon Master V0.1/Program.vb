#If WINDOWS Or XBOX Then
Imports System.Windows.Forms
Module Program
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Sub Main(ByVal args As String())
        Using game As New Game1()
            game.Run()
        End Using
    End Sub
End Module

#End If
