'NPC class
Public Class NPC

    'A NPC is similar to a player, excepted it can be controlled, use pokeballs or move
    Inherits Player

    'Creates a new NPC
    Sub New(ByVal nx As Integer, ByVal ny As Integer, ByRef pokedex As List(Of Pokemon), ByRef itemdex As List(Of Item), ByRef existing As Boolean)

        'Sets the X position
        setx(nx)

        'Sets the Y position
        sety(ny)

        'Sets the name to NPC so it can be indentifed
        setName("NPC")

        'If the NPC isn't being loaded then
        If existing = False Then

            'How many extra items and pokemon the NPC can get, by default the minimum is 1
            Const MaxNumberOfPokemonAndItemAdditional As Integer = 2

            'Determines a random number  of 1 + (0/1/2)
            Dim numberOfPokemon As Integer = 1 + CInt((Math.Floor(Rnd() * MaxNumberOfPokemonAndItemAdditional)))

            'Stores the current pokemons position in the pokedex
            Dim randomPokemonPosition As Integer

            'Loops for each item/pokemon
            For p = 0 To numberOfPokemon - 1

                'Generates a random seed
                Randomize()

                'Assign as new random pokedex position
                randomPokemonPosition = CInt(Math.Floor(Rnd() * pokedex.Count))

                'Gives the NPC a random pokeomn
                getpokemon(New Pokemon(pokedex(randomPokemonPosition).giveName, pokedex(randomPokemonPosition).giveLevel, pokedex(randomPokemonPosition).giveMaxHP, pokedex(randomPokemonPosition).giveMoves(0), pokedex(randomPokemonPosition).giveMoves(1), pokedex(randomPokemonPosition).giveMoves(2), pokedex(randomPokemonPosition).giveMoves(3), pokedex(randomPokemonPosition).giveType, pokedex(randomPokemonPosition).giveAttack, pokedex(randomPokemonPosition).giveDefence, pokedex(randomPokemonPosition).giveSpeed, pokedex(randomPokemonPosition).giveTextureAsString, pokedex(randomPokemonPosition).giveID))

                'Gives the NPC a radnom item
                getitem(itemdex(CInt(Math.Floor(Rnd() * (itemdex.Count)))))

            Next

        End If

    End Sub


    'The NPC interaction when the player is on top of it and presses T
    Public Overrides Sub interact()

        'Gets the default sub from player, so will do anything in the inherited sub
        MyBase.interact()

        'Changes game state to indicate the player is in a battle
        Game1.gameState = "Battle"

        'Creates a new battle
        Game1.gameBattle = New Battle(Game1.player, Me, False)

    End Sub



    'Returns all the chest attributes as a colon deliminated string
    Public Overrides Function getSaveString() As String

        Dim saveString As String = ""

        'Saves name
        saveString = saveString + getName() + ":"

        'Save bag count
        saveString = saveString + CStr(bag.Count) + ":"

        For b = 0 To bag.Count - 1

            'Saves item IDs
            saveString = saveString + CStr(bag(b).getID) + ":"

        Next

        'Saves pokemon count
        saveString = saveString + CStr(getPokemonList.Count)

        For p = 0 To getPokemonList.Count - 1

            'Saves pokemon save string
            saveString = saveString + ":" + getPokemonList(p).getSaveString

        Next

        'Returns the string
        Return saveString

    End Function


End Class
