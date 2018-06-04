'Class to store and display in game GUIs
Public Class MessageBox

    'Stores message box text
    Private text As List(Of String)

    'Constant for the message box texture height in pixels
    Private textBoxHeight As Integer = 44

    'Constant for the message box texture width in pixels
    Private textBoxWidth As Integer = 250

    'Constant for the pointer texture height in pixels
    Private pointerHeight As Integer = 15

    'Constant for the pointer texture width in pixels
    Private pointerWidth As Integer = 19

    'Constant for textbox size multiplier - for signs
    Private textBoxMultiplier As Decimal = CDec(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth + Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) / 2) * (1.5 / 512))

    'How far in pxiels is the text offset X direction
    Private textOffsetWidth As Integer = 10

    'How far in pxiels is the text offset Y direction
    Private textOffsetHeight As Integer = 4

    'Textbox texture
    Private _textBoxTexture As Texture2D

    'Pointer texture
    Private _pointerTexture As Texture2D

    'Boolean if the text in the message box is displayed
    Private displayTextBoolean As Boolean

    'Boolean if the pointer in the message box is displayed
    Private displayPointerBoolean As Boolean

    'Pointer position
    Private pointerPosition As Vector2

    'Boolean if the message box is displayed
    Private displayMessageBox As Boolean

    'Stores pokemon selection from - pokemon list/ pokemon info menu
    Private selectedPokemon As Pokemon


    'Creates a new message box
    Sub New(ByVal listOfNewText As List(Of String), ByVal displayBooleanText As Boolean, ByVal newDisplayPointerBoolean As Boolean)

        'Assigns the message boxes text
        text = listOfNewText

        'Tries to execute the next code
        Try

            'Loads the textbox texture
            _textBoxTexture = Game1.ContentLoader.Load(Of Texture2D)("textbox")

            'Loads the pointer texture
            _pointerTexture = Game1.ContentLoader.Load(Of Texture2D)("pointer")

            'Catches any crashes
        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading message box textures. " + ex.Message)

            'Returns player to map
            Game1.gameState = "Playing"

        End Try

        'Assigns boolean
        displayTextBoolean = displayBooleanText

        Me.displayPointerBoolean = newDisplayPointerBoolean

        pointerPosition = New Vector2(0, 0)

    End Sub


    'Updates message box logic
    Sub update()

        'Get which key has been pressed
        Game1.kbState = Keyboard.GetState

        If Game1.gameState = "Menu" Then

            'Sets number of pointer posions Y direction = to number of menu  options
            SetPointerY(text.Count)

            If Game1.kbState.IsKeyDown(Keys.Enter) Then

                Select Case pointerPosition.Y

                    Case 0

                        pointerPosition = New Vector2(0, 0)

                        'Plays sound effect
                        Game1.pressA.Play()

                        'Shows what pokemon player has
                        Game1.gameState = "DisplayPokemon"

                    Case 1

                        pointerPosition = New Vector2(0, 0)

                        'Plays sound effect
                        Game1.pressA.Play()

                        'Shows what items player has
                        Game1.gameState = "DisplayBag"

                    Case 2

                        'Plays sound effect
                        Game1.pressA.Play()

                        'Saves the game
                        Game1.gameState = "Save"

                    Case 3

                        'Stops current playing music - ilde song
                        MediaPlayer.Stop()

                        Game1.playingSong = False

                        'Plays sound effect
                        Game1.pressA.Play()

                        'Returns user to main menu
                        Game1.gameState = "MainMenu"

                End Select

            ElseIf Game1.kbState.IsKeyDown(Keys.E) Then

                'Plays sound effect
                Game1.pressA.Play()

                'Exits menu
                Game1.gameState = "Playing"

            End If

        ElseIf Game1.gameState = "DisplayPokemon" Then

            'Size possible pokemon in total - 3 down
            SetPointerY(3)

            'Two across
            SetPointerX(2)

            If Game1.kbState.IsKeyDown(Keys.E) Then

                'Plays sound effect
                Game1.pressA.Play()

                'Exits menu
                Game1.gameState = "Menu"

            ElseIf Game1.kbState.IsKeyDown(Keys.Enter) And Game1.player.getPokemonList.Count > 0 Then

                Try

                    'Gets pokemon at pointer position
                    selectedPokemon = Game1.player.getPokemonList(getPokemonPosition)

                    'Plays sound effect
                    Game1.pressA.Play()

                    'Displays pokemons info
                    Game1.gameState = "PokemonInfo"

                    'Catches any crashes
                Catch ex As Exception

                End Try

            ElseIf Game1.kbState.IsKeyDown(Keys.R) And Game1.player.getPokemonList.Count > 1 Then

                Try

                    'Plays sound effect
                    Game1.pressA.Play()

                    'Removes pokemon at pointer position
                    Game1.player.removePokemon(getPokemonPosition)

                Catch ex As Exception

                End Try

            End If

        ElseIf Game1.gameState = "Sign" Then

            'Exits sign
            If Game1.kbState.IsKeyDown(Keys.T) Then

                'Plays sound effect
                Game1.pressA.Play()

                Game1.gameState = "Playing"

            End If

            'Exits bag
        ElseIf Game1.gameState = "DisplayBag" Then

            If Game1.kbState.IsKeyDown(Keys.E) Then

                'Plays sound effect
                Game1.pressA.Play()

                Game1.gameState = "Menu"

            End If

            'Exits pokemon info
        ElseIf Game1.gameState = "PokemonInfo" Then

            If Game1.kbState.IsKeyDown(Keys.E) Then

                'Plays sound effect
                Game1.pressA.Play()

                Game1.gameState = "Playing"

            End If

        ElseIf Game1.gameState = "Chest" Then

            'Six posiible itmes three down two across
            SetPointerY(6)

            SetPointerX(2)

            'Exits chest
            If Game1.kbState.IsKeyDown(Keys.T) Then

                'Plays sound effect
                Game1.pressA.Play()

                Game1.gameState = "Playing"

            ElseIf Game1.kbState.IsKeyDown(Keys.Enter) Then

                'Gives player/chest pointer selected item6

                transferItem()

            End If

        End If

    End Sub


    'Transfers item from Chest inventory to player inventory and player inventory to chest inventory
    Sub transferItem()

        'Gets the pointers X position - left is players inventory, right is chest inventory
        Select Case pointerPosition.X

            Case 0

                Try

                    'If the chest doesn't have a full inventory
                    If Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag.Count < 6 Then

                        'Gives the chest the item
                        Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getitem(Game1.player.getBag(CInt(pointerPosition.Y)))

                        'Removes the item from the player
                        Game1.player.removeItem(CInt(pointerPosition.Y))

                    End If

                Catch ex As Exception

                End Try

            Case 1

                Try

                    'If the player doesn't have a full inventory
                    If Game1.player.getBag.Count < 6 Then

                        Game1.player.getitem(Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag(CInt(pointerPosition.Y)))

                        'Removes the item from the chest
                        Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).removeItem(CInt(pointerPosition.Y))

                        'Gives the player the item
                        Game1.recieveItem.Play()

                    End If

                Catch ex As Exception

                End Try

        End Select

    End Sub


    'Returns the pointer positon as a position in the pokemon list
    Function getPokemonPosition() As Integer

        Select Case pointerPosition

            Case New Vector2(0, 0)

                Return 0

            Case New Vector2(0, 1)

                Return 1

            Case New Vector2(0, 2)

                Return 2

            Case New Vector2(1, 0)

                Return 3

            Case New Vector2(1, 1)

                Return 4

            Case New Vector2(1, 2)

                Return 5

            Case Else

                Return -1

        End Select

    End Function


    'Change updates the pointers Y position with an upper bound
    Sub SetPointerY(ByVal Ymax As Integer)

        'Decrease Y position if down arrow is pressed
        If Game1.kbState.IsKeyDown(Keys.Down) Then

            pointerPosition.Y += 1

            'Increasse Y position if up arrow is pressed
        ElseIf Game1.kbState.IsKeyDown(Keys.Up) Then

            pointerPosition.Y -= 1

        End If

        'If the Y position is below 0 then it will set it back to 0 so it isn't out of bounds
        If pointerPosition.Y < 0 Then

            pointerPosition.Y = 0

        End If

        'If Y position is over the max limit it will set the pointer Y position to the max so the Y position is in the bounds
        If pointerPosition.Y > Ymax - 1 Then

            pointerPosition.Y = Ymax - 1

        End If

    End Sub


    'Change updates the pointers X position with an upper bound
    Sub SetPointerX(ByVal Xmax As Integer)

        'Decrease X position if left arrow is pressed
        If Game1.kbState.IsKeyDown(Keys.Left) Then

            pointerPosition.X -= 1

            'Increase X position if right arrow is pressed
        ElseIf Game1.kbState.IsKeyDown(Keys.Right) Then

            pointerPosition.X += 1

        End If

        'If the X position is below 0 then it will set it back to 0 so it isn't out of bounds
        If pointerPosition.X < 0 Then

            pointerPosition.X = 0

        End If

        'If X position is over the max limit it will set the pointer Y position to the max so the Y position is in the bounds
        If pointerPosition.X > Xmax - 1 Then

            pointerPosition.X = Xmax - 1

        End If

    End Sub


    'Draws the message box
    Sub draw(ByRef Tiles As SpriteBatch)

        Try

            Tiles.Begin()

            Dim destinationRectangle, sourceRectange As Rectangle

            If Game1.gameState = "Sign" Then

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                destinationRectangle = New Rectangle(CInt((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2), CInt((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - ((textBoxHeight * textBoxMultiplier) / 2)), CInt(textBoxWidth * textBoxMultiplier), CInt(textBoxHeight * textBoxMultiplier + ((textBoxHeight * textBoxMultiplier) / 2)))

                'Draws textbox texture
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                If displayTextBoolean = True Then

                    For i = 0 To text.Count - 1

                        'Draws text
                        Tiles.DrawString(Game1.font, text(i), New Vector2(CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2) + (textOffsetWidth * textBoxMultiplier)), CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - ((textBoxHeight * textBoxMultiplier) / 2))) + (textOffsetHeight * textBoxMultiplier) + (i * 10)), Color.Black)

                    Next

                End If

            ElseIf Game1.gameState = "Menu" Then

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                'Menu size on screen
                destinationRectangle = New Rectangle(10, 10, 88, 200)

                'Draws menu textbox
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                If displayTextBoolean = True Then

                    For i = 0 To text.Count - 1

                        'Draws menu options
                        Tiles.DrawString(Game1.font, text(i), New Vector2(30, 20 + i * 15), Color.Black)


                    Next

                End If

                If displayPointerBoolean = True Then

                    sourceRectange = New Rectangle(0, 0, pointerWidth, pointerHeight)

                    destinationRectangle = New Rectangle(10, CInt(20 + pointerPosition.Y * 15), pointerWidth, pointerHeight)

                    'Draws pointer
                    Tiles.Draw(_pointerTexture, destinationRectangle, sourceRectange, Color.White)

                End If

            ElseIf Game1.gameState = "DisplayPokemon" Then

                'Loads pokemon texture
                Dim pokemonSpriteTexture As Texture2D

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                destinationRectangle = New Rectangle(CInt(Math.Round(0.1 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.01 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)), CInt(Math.Round(0.78 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.97 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)))

                'Draws textbox
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                If displayPointerBoolean = True Then

                    sourceRectange = New Rectangle(0, 0, pointerWidth, pointerHeight)

                    destinationRectangle = New Rectangle(CInt(CInt(Math.Round(0.12 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)) + pointerPosition.X * 200), CInt(CInt(Math.Round(0.15 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)) + pointerPosition.Y * 150), pointerWidth, pointerHeight)

                    'Draws pointer
                    Tiles.Draw(_pointerTexture, destinationRectangle, sourceRectange, Color.White)

                End If

                For i = 0 To Game1.player.getPokemonList.Count - 1

                    pokemonSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(Game1.player.getPokemonList(i).getSpriteLocation)

                    sourceRectange = New Rectangle(0, 0, Game1.player.getPokemonList(i).getSpriteWidth, Game1.player.getPokemonList(i).getSpriteHeight)

                    If i < 3 Then


                        destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + 50), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) + (i * 150)), 100, 100)
                    Else


                        destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + (250)), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) + ((i Mod 3) * 150)), 100, 100)
                    End If

                    'Draws pokemon sprite
                    Tiles.Draw(pokemonSpriteTexture, destinationRectangle, sourceRectange, Color.White)

                Next

            ElseIf Game1.gameState = "DisplayBag" Then

                'Loads item sprites
                Dim itemSpriteTexture As Texture2D

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                destinationRectangle = New Rectangle(CInt(Math.Round(0.1 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.01 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)), CInt(Math.Round(0.78 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.97 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)))

                'Draws textbox
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                For i = 0 To Game1.player.getBag.Count - 1

                    itemSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(Game1.player.getBag(i).getSpriteLocation)

                    sourceRectange = New Rectangle(0, 0, Game1.player.getBag(i).getSpriteWidth, Game1.player.getBag(i).getSpriteHeight)

                    destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + 175), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) + (i * 75)), 50, 50)

                    'Draws items name
                    Tiles.DrawString(Game1.font, Game1.player.getBag(i).getName, New Vector2(CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2) + (textOffsetWidth * textBoxMultiplier)) + 125, CInt(80 + (i * 75))), Color.Black)

                    'Draws item sprite
                    Tiles.Draw(itemSpriteTexture, destinationRectangle, sourceRectange, Color.White)

                Next

            ElseIf Game1.gameState = "PokemonInfo" Then

                Dim pokemonSpriteTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("pokemon")

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                destinationRectangle = New Rectangle(CInt(Math.Round(0.1 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.01 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)), CInt(Math.Round(0.78 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.97 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)))

                'Draws textbox texture
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                pokemonSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(selectedPokemon.getSpriteLocation)

                sourceRectange = New Rectangle(0, 0, selectedPokemon.getSpriteWidth, selectedPokemon.getSpriteHeight)

                destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + 150), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)), 100, 100)

                'Draws pokemon sprite
                Tiles.Draw(pokemonSpriteTexture, destinationRectangle, sourceRectange, Color.White)

                text = selectedPokemon.giveStatsAsList

                For i = 0 To text.Count - 1

                    'Draws pokemons info
                    Tiles.DrawString(Game1.font, text(i), New Vector2(CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2) + (textOffsetWidth * textBoxMultiplier)) + 100, CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - ((textBoxHeight * textBoxMultiplier) / 2))) + (textOffsetHeight * textBoxMultiplier) + (i * 10)), Color.Black)

                Next

            ElseIf Game1.gameState = "Chest" Then

                'Loads item sprite
                Dim itemSpriteTexture As Texture2D

                sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

                destinationRectangle = New Rectangle(CInt(Math.Round(0.1 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.01 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)), CInt(Math.Round(0.78 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)), CInt(Math.Round(0.97 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)))

                'Draws textbox texture
                Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

                If displayPointerBoolean = True Then

                    sourceRectange = New Rectangle(0, 0, pointerWidth, pointerHeight)

                    destinationRectangle = New Rectangle(CInt(CInt(Math.Round(0.12 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)) + pointerPosition.X * 200), CInt(CInt(Math.Round(0.11 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight)) + pointerPosition.Y * 75), pointerWidth, pointerHeight)

                    'Draws pointer
                    Tiles.Draw(_pointerTexture, destinationRectangle, sourceRectange, Color.White)

                End If

                For i = 0 To Game1.player.getBag.Count - 1

                    itemSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(Game1.player.getBag(i).getSpriteLocation)

                    sourceRectange = New Rectangle(0, 0, Game1.player.getBag(i).getSpriteWidth, Game1.player.getBag(i).getSpriteHeight)

                    destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + 50), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) + (i * 75)), 50, 50)

                    'Draws item sprite - player
                    Tiles.Draw(itemSpriteTexture, destinationRectangle, sourceRectange, Color.White)
                Next

                For i = 0 To Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag.Count - 1

                    itemSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag(CInt(i)).getSpriteLocation)

                    sourceRectange = New Rectangle(0, 0, Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag(i).getSpriteWidth, Game1.currentMap.getEntity(Game1.player.givex, Game1.player.givey).getBag(CInt(i)).getSpriteHeight)

                    destinationRectangle = New Rectangle(CInt(Math.Round(0.06 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth) + 250), CInt(Math.Round(0.08 * Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) + (i * 75)), 50, 50)

                    'Draws itme sprite - chest
                    Tiles.Draw(itemSpriteTexture, destinationRectangle, sourceRectange, Color.White)

                Next

            End If

            Tiles.End()

            'Catches any crashes
        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading textbox. " + ex.Message)

        End Try

    End Sub


End Class