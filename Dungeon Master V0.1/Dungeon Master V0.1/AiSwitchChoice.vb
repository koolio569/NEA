'A class to store the percentage chance of the SwitchAi choosing a specific pokemon and the location within the enemy's pokemon list
Public Class AI_Switch_Choice
    'Stores the percentage chance as a decimal
    Public value As Decimal
    'Stores the position in the enemy's pokemon list, byte was chosen as maxmimum pokemon list size is 6
    Public position As Byte


    'Creates a new switch choice
    Sub New(ByVal newValue As Decimal, ByVal newPosition As Byte)
        value = newValue
        position = newPosition
    End Sub
End Class