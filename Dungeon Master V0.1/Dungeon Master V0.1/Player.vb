'Player class - used for the user/ npc's/ chests/ signs and encounter pokemons
Public Class Player

    'Stores players name
    Protected name As String

    'Stores players pokemon - max 6
    Protected pokemons As New List(Of Pokemon)

    'Stores players items - max 6
    Protected bag As New List(Of Item)

    'Stores players X position in the map
    Protected playerXCoordinate As Integer

    'Stores players Y position in the map
    Protected playerYCoordinate As Integer

    'Stores the players X position on the screen
    Protected playerPixelXCoordinate As Integer

    'Stores the players Y position on the screen
    Protected playerPixelYCoordinate As Integer


    'Creates a new player
    Sub New()

        'Defaults player location to the top left of the maze
        playerXCoordinate = 0

        playerYCoordinate = 0

        'Defaults name - refernce to Ash Ketchum
        name = "Cole Mustard"

    End Sub


    'Sets the players name
    Sub setName(ByRef newName As String)

        name = newName

    End Sub


    'Sets players X position in the map
    Sub setx(ByVal value As Integer)

        playerXCoordinate = value

    End Sub


    'Sets players Y position in the map
    Sub sety(ByVal value As Integer)

        playerYCoordinate = value

    End Sub


    'Sets players X position on the screen
    Sub setpx(ByVal value As Integer)

        playerPixelXCoordinate = value

    End Sub


    'Sets players Y position on the screen
    Sub setpy(ByVal value As Integer)
        playerPixelYCoordinate = value
    End Sub


    'Removes an item from the players bag, at a given position
    Sub removeItem(ByVal position As Integer)

        bag.RemoveAt(position)

    End Sub


    'Removes a pokemon from the players pokemon list, at a given position
    Sub removePokemon(ByVal position As Integer)

        pokemons.RemoveAt(position)

    End Sub


    'The default sub for a player interaction - user player and encounter pokemon cant be interacted with
    Overridable Sub interact()

        ' NPC interaction

    End Sub


    'Gives the user a pokemon
    Sub getpokemon(ByVal NewPokemon As Pokemon)

        pokemons.Add(NewPokemon)

    End Sub


    'Gives the player an item
    Sub getitem(ByVal NewItem As Item)

        bag.Add(NewItem)

    End Sub


    'Returns players X position in the map
    Function givex() As Integer

        Return playerXCoordinate

    End Function


    'Returns players X position on the screen
    Function givepx() As Integer

        Return playerPixelXCoordinate

    End Function


    'Returns players Y position in the map
    Function givey() As Integer

        Return playerYCoordinate

    End Function


    'Returns players Y position on the screen
    Function givepy() As Integer

        Return playerPixelYCoordinate

    End Function


    'Returns the players name
    Function getName() As String

        Return name

    End Function


    'Returns the players bag as a list of items
    Function getBag() As List(Of Item)

        Return bag

    End Function


    'Returns the players pokemon list
    Function getPokemonList() As List(Of Pokemon)

        Return pokemons

    End Function


    'Returns all the pokemons Hp and names the player has in a list of strings
    Function getPokemonNamesAndHP() As List(Of String)

        'Stores the HP and names as 1 item in the list
        Dim listOfPokemonNames As New List(Of String)

        For i = 0 To pokemons.Count - 1

            'Adds the name and hp to the list
            listOfPokemonNames.Add(pokemons(i).giveName + " - HP: " + CStr(pokemons(i).giveHP))

        Next

        'Returns the list
        Return listOfPokemonNames

    End Function


    'Returns the names of the items in the players bag as a list of strings
    Function getItemNames() As List(Of String)

        'Stores the itme names
        Dim listOfItemNames As New List(Of String)

        For i = 1 To bag.Count

            'Adds the names to the list
            listOfItemNames.Add(bag(i - 1).getName)

        Next

        'Returns the list
        Return listOfItemNames

    End Function


    'Checks if there is an item in the players bag, with the name containg a query, eg. input potion i will check if there is a potion as well
    'As a super or hyper potion
    Function haveItem(ByVal itemName As String) As Boolean

        'Gets a list of all the item names in the players bag
        Dim nameOfItemsInBag As List(Of String) = getItemNames()

        For i = 0 To nameOfItemsInBag.Count - 1

            'Checks if the query is in the name
            If nameOfItemsInBag(i).Contains(itemName) Then

                'If it finds the first instance then it will return true
                Return True

            End If

        Next

        'If it doesn't find it, it will return false
        Return False

    End Function


    'Updates the players logic - movement and collision
    Public Sub update()

        'Gets the current keyboard state, to see which key has been pressed if any
        Game1.kbState = Keyboard.GetState

        If Game1.kbState.IsKeyDown(Keys.W) Then

            'If the player isn't at the top of the map the maze
            If Not playerPixelYCoordinate = 0 Then

                'If the player isn't at a cell boundary
                If Not playerPixelYCoordinate Mod 16 = 0 Then

                    'Moves player up by 1 pixel
                    playerPixelYCoordinate -= 1

                    'Will check if the player can pass the cell boundary
                Else

                    'Collsion detection if at least one of the walls the player is trying walk through doesn't exist then the player can pass
                    'The cell cboundary
                    If Game1.currentMap.tile(playerXCoordinate, playerYCoordinate).getNorth = False Or Game1.currentMap.tile(playerXCoordinate, playerYCoordinate - 1).getSouth = False Then

                        'Moves the player up by 1 pixel
                        playerPixelYCoordinate -= 1

                        'Converts the players pixel Y position to a maze Y position
                        playerYCoordinate = CInt(Math.Floor(playerPixelYCoordinate / 16))

                    End If

                End If

            End If

        ElseIf Game1.kbState.IsKeyDown(Keys.S) Then

            'If the player isn't at the bottom of the maze
            If Not playerPixelYCoordinate >= (Game1.currentMap.getHeight * 16) - 1 Then

                'If the player isn't at a cell boundary
                If Not playerPixelYCoordinate Mod 16 = 15 Then

                    'Moves the player down by 1 pixel
                    playerPixelYCoordinate += 1

                    'Else it will check if the player can pass through the boundary
                Else

                    'If at least one of the walls doesnt't exist then the player can pass the boundary
                    If Game1.currentMap.tile(playerXCoordinate, playerYCoordinate).getSouth = False Or Game1.currentMap.tile(playerXCoordinate, playerYCoordinate + 1).getNorth = False Then

                        'Moves the player down by 1 pixel
                        playerPixelYCoordinate += 1

                        'converts the players Y pixel position to a maze Y position
                        playerYCoordinate = CInt(Math.Floor(playerPixelYCoordinate / 16))

                    End If

                End If

            End If

        ElseIf Game1.kbState.IsKeyDown(Keys.A) Then

            'If the player isn't at the most left of the map
            If Not playerPixelXCoordinate = 0 Then

                'If the player isn't at a cell boundary
                If Not playerPixelXCoordinate Mod 16 = 0 Then

                    'Moves the player 1 pixel to the left
                    playerPixelXCoordinate -= 1

                    'Else it will check if the player can pass throught the player boundary
                Else

                    'If at least one of the walls it is trying to pas through the player can pass through the boundary
                    If Game1.currentMap.tile(playerXCoordinate, playerYCoordinate).getWest = False Or Game1.currentMap.tile(playerXCoordinate - 1, playerYCoordinate).getEast = False Then

                        'Moves the player one pixel to the left
                        playerPixelXCoordinate -= 1

                        'Converts the players X pixel position to a X maze position
                        playerXCoordinate = CInt(Math.Floor(playerPixelXCoordinate / 16))

                    End If

                End If

            End If

        ElseIf Game1.kbState.IsKeyDown(Keys.D) Then

            'If the player isn't at the most right part of the maze
            If Not playerPixelXCoordinate >= (Game1.currentMap.getWidth * 16) - 1 Then

                'If the player isn't at a cell boundary
                If Not playerPixelXCoordinate Mod 16 = 15 Then

                    'Moves the player one pixel to the right
                    playerPixelXCoordinate += 1

                Else

                    'If at least one wall the player is trying to move through doesn't exist the player can pass through the boundary
                    If Game1.currentMap.tile(playerXCoordinate, playerYCoordinate).getEast = False Or Game1.currentMap.tile(playerXCoordinate + 1, playerYCoordinate).getWest = False Then

                        'Moves the player one pixel to the right
                        playerPixelXCoordinate += 1

                        'converts the players X pixel position to a maze X position
                        playerXCoordinate = CInt(Math.Floor(playerPixelXCoordinate / 16))

                    End If

                End If

            End If

        End If

    End Sub


    'Draws the players sprite
    Sub drawAll(ByRef playerSprite As SpriteBatch)

        'Texture size in pixels
        Const playerTextureSize As Integer = 16

        'How far the texture will be offset when drawn
        Const playerTextureOffset As Integer = 8

        Dim sourceRectange, destinationRectangle As Rectangle

        'Position in texture file where texture wil be taken from
        Dim playerTextureStartPosition As Vector2 = New Vector2(0, 0)

        Try

            'Loads the players Texture
            Dim _PlayerTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("playersprite")

            playerSprite.Begin()

            sourceRectange = New Rectangle(CInt(playerTextureStartPosition.X), CInt(playerTextureStartPosition.Y), playerTextureSize, playerTextureSize)

            destinationRectangle = New Rectangle(playerPixelXCoordinate - playerTextureOffset, playerPixelYCoordinate - playerTextureOffset, playerTextureSize, playerTextureSize)

            'Draws player sprite
            playerSprite.Draw(_PlayerTexture, destinationRectangle, sourceRectange, Color.White)

            playerSprite.End()

        Catch ex As Exception

            'Displays error messsage
            MsgBox("Error loading player texture, game will exit. " + ex.Message)

        End Try

    End Sub


    'Get player's save data as a string
    Overridable Function getSaveString() As String

        'Stores save data
        Dim saveString As String

        'Saves the players name
        saveString = name + ";"

        'Saves the players coordinates in the maze and on screen
        saveString = saveString + CStr(playerXCoordinate) + ";" + CStr(playerYCoordinate) + ";" + CStr(playerPixelXCoordinate) + ";" + CStr(playerPixelYCoordinate) + ";"

        'Saves number of items in players bag
        saveString = saveString + CStr(bag.Count) + ";"

        For b = 0 To bag.Count - 1

            'Saves current items id
            saveString = saveString + CStr(bag(b).getID) + ";"

        Next

        'Save number of pokemon player has
        saveString = saveString + CStr(pokemons.Count) + ";"

        For p = 0 To pokemons.Count - 1

            'Saves pokemon save data
            saveString = saveString + pokemons(p).getSaveString + ";"

        Next

        'Returns string
        Return saveString

    End Function


End Class
