'A class to cells which make up the map
Public Class Cell

    'X position in the map
    Private xPosition As Integer

    'Y position in the map
    Private yPosition As Integer

    'Boolean for the depth first search to store if it is visited or not
    Private visited As Boolean

    'Store if the north wall exists
    Private North As Boolean

    'Stores if the east wall exists
    Private East As Boolean

    'Stores if the south wall exists
    Private South As Boolean

    'Stores if the west wall exists
    Private West As Boolean

    'Stores the cells symbol, dependent on which walls exist
    Private symbol As Integer


    'Creates new cell, sets X and Y position, other attributes as defaulted
    Sub New(ByVal nx As Integer, ByVal ny As Integer)

        xPosition = nx

        yPosition = ny

        visited = False

        North = True

        East = True

        South = True

        West = True

        symbol = 0

    End Sub


    'Empties cell, all walls don't exist and has been visited
    Sub empty()

        visited = True

        North = False

        East = False

        South = False

        West = False

    End Sub


    'Returns X position
    Function getX() As Integer
        Return xPosition
    End Function


    'Returns Y position
    Function getY() As Integer
        Return yPosition
    End Function


    'Returns boolean Visited
    Function getVisited() As Boolean
        Return visited
    End Function


    'Returns North wall existence as boolean
    Function getNorth() As Boolean
        Return North
    End Function


    'Returns East wall existence as boolean
    Function getEast() As Boolean
        Return East
    End Function


    'Returns South wall existence as boolean
    Function getSouth() As Boolean
        Return South
    End Function


    'Returns West wall existence as boolean
    Function getWest() As Boolean
        Return West
    End Function


    'Returns cell's symbol
    Function getSymbol() As Integer
        Return symbol
    End Function

    'Sets X position from given value
    Sub setX(ByVal newX As Integer)
        xPosition = newX
    End Sub


    'Sets Y position from given value
    Sub setY(ByVal newY As Integer)
        yPosition = newY
    End Sub


    'Sets Visited from given value as boolean
    Sub setVisited(ByVal newVisited As Boolean)
        visited = newVisited
    End Sub


    'Sets North wall existence
    Sub setNorth(ByVal newNorth As Boolean)
        North = newNorth
    End Sub


    'Sets East wall existence
    Sub setEast(ByVal newEast As Boolean)
        East = newEast
    End Sub


    'Sets South wall existence
    Sub setSouth(ByVal newSouth As Boolean)
        South = newSouth
    End Sub


    'Sets West wall existence
    Sub setWest(ByVal newWest As Boolean)
        West = newWest
    End Sub


    'Sets cells's wall from a given symbol
    Sub setCellFromSymbol(ByVal newSymbol As Integer)

        'Converts symbol from denary to binary
        Dim symbolAsBinaryString As String = ""

        symbol = newSymbol

        While newSymbol <> 0

            If newSymbol Mod 2 = 0 Then

                symbolAsBinaryString = symbolAsBinaryString + "0"

            Else

                symbolAsBinaryString = symbolAsBinaryString + "1"

            End If

            newSymbol = newSymbol \ 2

        End While

        'If the symbols is less than 8 then it will add 0s to the string so it will be 4 bits
        If symbolAsBinaryString.Length < 4 Then

            Do

                symbolAsBinaryString = symbolAsBinaryString + "0"

            Loop Until symbolAsBinaryString.Length = 4

        End If

        'North is the most significant bit
        setNorth(wallTrueOrFalse(symbolAsBinaryString(0)))

        setEast(wallTrueOrFalse(symbolAsBinaryString(1)))

        setSouth(wallTrueOrFalse(symbolAsBinaryString(2)))

        'West is the least significant bit
        setWest(wallTrueOrFalse(symbolAsBinaryString(3)))

    End Sub

    'Returns True or False depending on the walls bit
    Function wallTrueOrFalse(ByRef wall As String) As Boolean

        If wall = "0" Then

            Return False

        Else

            Return True

        End If

    End Function


    'Converts the walls existence states to a denary number the walls were bits, true being 1 and false being 0. 
    'From most to least significant bit: North East South West
    'Converts binary to denary
    Sub setSymbol(ByVal n As Boolean, ByVal e As Boolean, ByVal s As Boolean, ByVal w As Boolean)


        Dim total As Byte = 0

        If n = True Then

            total = CByte(total + 1)

        End If

        If e = True Then

            total = CByte(total + 2)

        End If

        If s = True Then

            total = CByte(total + 4)

        End If

        If w = True Then

            total = CByte(total + 8)

        End If

        symbol = total

    End Sub


End Class
