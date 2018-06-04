'Pokemon class
Public Class Pokemon

    'Stores the pokemons uique id in then pokedex
    Private ID As Integer

    'Pokemons name
    Private name As String

    'Pokemons leavel
    Private level As Integer

    'How much health the pokemon has currently
    Private HP As Integer

    'How much health it begins with
    Private MaxHP As Integer

    'The 4 moves a pokemon has - max 4
    Private useableMoves(3) As Move

    'Pokemon type
    Private type As String

    'Pokemon attack stat - how good it is at damaging
    Private attack As Integer

    'Pokemon defence stat - how good it is at not taking damage
    Private defence As Integer

    'How fast the pokemon is
    Private speed As Integer

    'The pokemons sprite texture file location
    Private spriteLocation As String

    'Sprite width in pixels
    Private spriteWidth As Integer

    'Sprite height in pixels
    Private spriteHeight As Integer


    'Creates a new pokemon
    Sub New(ByVal newName As String, ByVal newLevel As Integer, ByVal newHP As Integer, ByVal move1 As Move, ByVal move2 As Move, ByVal move3 As Move, ByVal move4 As Move, ByVal newType As String, ByVal newAttack As Integer, ByVal newDefence As Integer, ByVal newSpeed As Integer, ByVal newTexture As String, ByVal newID As Integer)

        'Assigns the pokemons ID
        ID = CInt(newID)

        'Assigns the pokemons name
        name = newName

        'Assigns the pokemons level
        level = newLevel

        'Assigns the pokemons hp
        HP = newHP + (level * 2)

        'Assigns maxHp which is eqaul to hp as it hasn't taken any damage
        MaxHP = HP

        'Pokemons 1st move
        useableMoves(0) = move1

        'Pokemons 2nd move
        useableMoves(1) = move2

        'Pokemons 3rd move
        useableMoves(2) = move3

        'Pokemons 4th move
        useableMoves(3) = move4

        'Assigns pokemons type
        type = newType

        'Assigns pokemons attack stat
        attack = newAttack + (level * 2)

        'Assigns pokemons defence stat
        defence = newDefence + (level * 2)

        'Assigns pokemons speed stat
        speed = newSpeed + (level * 2)

        'Stores pokemons sprite information
        Dim currentSpriteInfo() As String

        'Splits the sprite information into an array, by colons as information is colon delimnated
        currentSpriteInfo = newTexture.Split(CChar(":"))

        'Assigns sprite location
        spriteLocation = currentSpriteInfo(0)

        'Assigns sprite width
        spriteWidth = CInt(currentSpriteInfo(1))

        'Assigns sprite height
        spriteHeight = CInt(currentSpriteInfo(2))

    End Sub


    'Returns pokemons ID
    Function giveID() As Integer

        Return ID

    End Function


    'Returns pokemons sprite inforation as a colon deliminated string
    Function giveTextureAsString() As String

        Return spriteLocation + ":" + CStr(spriteWidth) + ":" + CStr(spriteHeight)

    End Function


    'Returns pokemons name
    Function giveName() As String

        Return name

    End Function


    'Returns pokemons level
    Function giveLevel() As Integer

        Return level

    End Function


    'Returns pokemons current health
    Function giveHP() As Integer

        Return HP

    End Function


    'Returns pokemons moves as an array
    Function giveMoves() As Move()

        Return useableMoves

    End Function


    'Returns pokemons type
    Function giveType() As String

        Return type

    End Function


    'Returns the health the pokemon started with
    Function giveMaxHP() As Integer

        Return MaxHP

    End Function


    'Returns pokemons speed stat
    Function giveSpeed() As Integer

        Return speed

    End Function


    'Returns pokemons attack stat
    Function giveAttack() As Integer

        Return attack

    End Function


    'Returns pokemons defence stat
    Function giveDefence() As Integer

        Return defence

    End Function


    'Returns the pokemons stats as a list of string
    Function giveStatsAsList() As List(Of String)

        Return New List(Of String) From {CStr(name), "Level: " + CStr(level), "HP: " + CStr(HP), "Type: " + CStr(type), CStr(useableMoves(0).GiveName), CStr(useableMoves(1).GiveName), CStr(useableMoves(2).GiveName), CStr(useableMoves(3).GiveName)}

    End Function


    'Returns the pokemons moves names and PP as a list of string
    Function getMovesAndPPAsString() As List(Of String)

        Return New List(Of String) From {useableMoves(0).GiveName + " PP: " + CStr(useableMoves(0).givePP), useableMoves(1).GiveName + " PP: " + CStr(useableMoves(1).givePP), useableMoves(2).GiveName + " PP: " + CStr(useableMoves(2).givePP), useableMoves(3).GiveName + " PP: " + CStr(useableMoves(3).givePP)}

    End Function


    'Returns pokemons sprite location
    Function getSpriteLocation() As String

        Return spriteLocation

    End Function


    'Returns the pokemons sprite width in pixels
    Function getSpriteWidth() As Integer

        Return spriteWidth

    End Function


    'Returns sprite height in pixels
    Function getSpriteHeight() As Integer

        Return spriteHeight

    End Function


    'Sets pokemons health to a given value
    Sub SetHealth(ByRef h As Integer)

        HP = h

    End Sub


    'Sets the pokemons level to a given value
    Sub SetLevel(ByRef l As Integer)

        level = l

    End Sub


    'Sets the pokemons attack set to a given value
    Sub SetAttack(ByRef a As Integer)

        attack = a

    End Sub


    'Sets the pokemons defence stat from a given value
    Sub SetDefence(ByRef d As Integer)

        defence = d

    End Sub


    'Sets the pokemons speed stat from a given value
    Sub SetSpeed(ByRef s As Integer)

        speed = s

    End Sub


    'Sets a move to a given move at a given position
    Sub setMove(ByRef poisition As Integer, ByVal newMove As Move)

        useableMoves(poisition) = newMove

    End Sub


    'Decreases the pokemons health my a given amount
    Sub takeDamage(ByVal damage As Integer)

        HP = HP - damage

        If HP < 0 Then

            HP = 0

        End If

    End Sub

    'Gets the save data for the pokemon - all the attributes
    Function getSaveString() As String

        Return CStr(ID) + "," + CStr(level) + "," + CStr(HP) + "," + useableMoves(0).getSaveString + "," + useableMoves(1).getSaveString + "," + useableMoves(2).getSaveString + "," + useableMoves(3).getSaveString + "," + CStr(attack) + "," + CStr(defence) + "," + CStr(speed)

    End Function


End Class

