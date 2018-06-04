'A sign class interactable tile entity
Public Class Sign

    'Its like a player as it has a name
    Inherits Player

    'Stores the signs text
    Private text As List(Of String)

    'Booelean to represent if the text is displayed or not
    Private displayText As Boolean


    'Creates a new sign
    Sub New(ByVal nx As Integer, ByVal ny As Integer, ByVal newText As String)

        'Sets the X position in the map
        setx(nx)

        'Sets the Y position in the map
        sety(ny)

        'Sets the signs name so it can be identified
        setName("SIGN")

        'Defaults display text to false
        displayText = False

        'Adds text to the signs text list
        text = Game1.convertToList(newText)


    End Sub


    'The signs interaction when the player is on top of it and presses T
    Public Overrides Sub interact()


        'Changes game state to indicate the player is in the sign GUI
        Game1.gameState = "Sign"

        'Creates a new signs GUI
        Game1.messageBox = New MessageBox(text, True, False)

    End Sub


    'Returns the save data of the singn as a string
    Public Overrides Function getSaveString() As String

        Dim saveString As String = ""

        'Saves signs name - "SIGN"
        saveString = saveString + getName() + ":"

        For t = 0 To text.Count - 1

            'Saves the signs text
            saveString = saveString + text(t)

        Next

        Return saveString

    End Function


End Class
