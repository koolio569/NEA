'Class which stores the Chest entity
Public Class Chest

    'A chest is like a player, which only has a bag
    Inherits Player

    'Creates a new chest
    Sub New(ByVal nx As Integer, ByVal ny As Integer, ByRef itemdex As List(Of Item), ByRef existing As Boolean)

        'Sets the chests X position
        setx(nx)

        'Sets the chests Y position
        sety(ny)

        'Sets the chests's name to "CHEST" so it can be identified
        setName("CHEST")

        'If the Chest isn't being loaded from a file, then it will execute the next code
        If existing = False Then

            'Max number of item as= chest can have
            Const MaxNumberOfItems As Integer = 5

            'Choose a random number of items the chest has upto the maximum value
            Dim numberOfPokemon As Integer = CInt(Math.Floor(Rnd() * MaxNumberOfItems))

            'Loops for the number itmes the chest has
            For p = 0 To numberOfPokemon - 1

                'Generates a new random seed
                Randomize()

                'Gives the chest a random item
                getitem(itemdex(CInt(Math.Floor(Rnd() * (itemdex.Count - 1)))))

            Next

        End If

    End Sub


    'The chest interaction when the player is on top of it and presses T
    Public Overrides Sub interact()

        'Gets the default sub from player, so will do anything in the inherited sub
        MyBase.interact()

        'Changes game state to indicate the player is in the chest GUI
        Game1.gameState = "Chest"

        'Creates a new chest GUI
        Game1.messageBox = New MessageBox(New List(Of String), False, True)

    End Sub


    'Returns all the chest attributes as a colon deliminated string
    Public Overrides Function getSaveString() As String

        'Defaults the saveString
        Dim saveString As String = ""

        'Adds the chests's name "CHEST"
        saveString = saveString + getName() + ":"

        'Adds how many items the chest has
        saveString = saveString + CStr(bag.Count)

        'Loops through all the itmes in the chest's bag
        For b = 0 To bag.Count - 1

            'Adds the current item's ID
            saveString = saveString + ":" + CStr(bag(b).getID)

        Next

        Return saveString

    End Function
End Class
