Imports System.Windows.Forms

''' <summary>
''' This is the main type for your game
''' </summary>
''' 

Public Class Game1

    Inherits Microsoft.Xna.Framework.Game

    'Handles the configuration and management of the graphics device
    Public Shared WithEvents graphics As GraphicsDeviceManager

    'Stores sprites to be rendered
    Private WithEvents TileTextures As SpriteBatch

    'Stores current game state, controls which game logic/ drawing
    Public Shared gameState As String

    'Stores they keyboars inputs
    Public Shared kbState As KeyboardState

    Public Shared ContentLoader As ContentManager

    'The font which is used for in game text
    Public Shared font As SpriteFont

    'Stores the games battle/ encounters
    Public Shared gameBattle As Battle

    'Stores the games map
    Public Shared currentMap As Map

    'The player in the game
    Public Shared player As Player

    'A pop textbox/ menus/ GUIS
    Public Shared messageBox As MessageBox

    'Stores all pokemons in the game
    Public Shared pokedex As List(Of Pokemon)

    'Stores all the moves a pokemon can havein the game
    Public Shared movedex As List(Of Move)

    'Stores all the items in the game
    Public Shared itemdex As List(Of Item)

    'Boolean to determine if a song is playing or not
    Public Shared playingSong As Boolean

    'Sound effect for when player selects a main menu option or interacts with a NPC/CHEST/SIGN
    Public Shared pressA As SoundEffect

    'Sound effect for when the player gets an item
    Public Shared recieveItem As SoundEffect

    'An enemy specifically for encounters in tall grass, has one encounter pokemon
    Public Shared encounterPokemon As Player

    'Map height
    Const MapHeight As Integer = 10

    'Map width
    Const MapWidth As Integer = 10

    'The current song playing idle/Main menu/Battle
    Private currentSong As Song

    'File location for data for game1.pokedex
    Const pokedexFileLocation As String = "content/pokemon.csv"

    'File location for data for movedex
    Const movedexFileLocation As String = "content/Move index.csv"

    'File location for data for itemdex
    Const itemdexFileLocation As String = "Content/items.csv"

    'File location for sprite font
    Const fontFileLocation As String = "font"

    'File location for idle song
    Const idleSongLocation As String = "idleSong"

    'File location for battle song
    Const battleSongLocation As String = "battleSong"

    'File location for main menu song
    Const mainMenuSongLocation As String = "mainMenuSong"

    'File location for recieve item sound effect
    Const recieveItemEffectLocation As String = "recieveItem"

    'File location for selection sound effect
    Const pressAEffectLocation As String = "pressA"


    'Creates new game
    Public Sub New()
        graphics = New GraphicsDeviceManager(Me)
        Content.RootDirectory = "Content"
        graphics.PreferredBackBufferWidth = 512
        graphics.PreferredBackBufferHeight = 512
        playingSong = False
        graphics.ApplyChanges()
    End Sub


    ''' <summary>
    ''' Allows the game to perform any initialization it needs to before starting to run.
    ''' This is where it can query for any required services and load any non-graphic
    ''' related content.  Calling MyBase.Initialize will enumerate through any components
    ''' and initialize them as well.
    ''' </summary>
    ''' 

    Protected Overrides Sub Initialize()
        ' TODO: Add your initialization logic here
        MyBase.Initialize()

        'Loads content
        ContentLoader = Content

        'Creates new player
        player = New Player

        'Instantiate game1.pokedex
        game1.pokedex = New List(Of Pokemon)

        'Instantiate movedex
        movedex = New List(Of Move)

        'Instantiate itemdex
        itemdex = New List(Of Item)

        'Instantiate messageBox
        messageBox = New MessageBox(New List(Of String), False, False)

        'Loads data from movedex file into movedex list
        LoadMoves(movedexFileLocation)

        'Loads data from game1.pokedex file into game1.pokedex list
        LoadPokemon(game1.pokedexFileLocation)

        'Loads data from itemdex file into itemdex list
        loadItems(itemdexFileLocation)

        gameState = "MainMenu"

        Try

            'Loads spriteFont
            font = Content.Load(Of SpriteFont)(fontFileLocation)

        Catch ex As Exception

            MsgBox("Error loading Font file. " + ex.Message)

            gameState = "Exit"

        End Try

        Try

            'Loads sound effects
            pressA = Content.Load(Of SoundEffect)(pressAEffectLocation)

            recieveItem = Content.Load(Of SoundEffect)(recieveItemEffectLocation)

        Catch ex As Exception

            MsgBox("Error loading sound effect files. " + ex.Message)

            gameState = "Exit"

        End Try

        MsgBox("Press 1 to exit, 2 to load a save file or 3 to start a new game.")

    End Sub


    'Loads item file data into itemdex
    Sub loadItems(ByVal fileLocation As String)

        Try

            'Loads item file
            Dim itemFile As New IO.StreamReader(fileLocation)

            'List to store file lines
            Dim items As New List(Of String)

            'Stores current item
            Dim currentItem() As String

            'Loops until end of file
            While Not itemFile.EndOfStream

                'Adds file line to list
                items.Add(itemFile.ReadLine)

            End While

            'Closes item file as it is no longer required
            itemFile.Close()

            'Loops for each item in the file line list, excpet the first line which are titles
            For i = 1 To items.Count - 1

                'Splits the current file line by commas as file is comma deliminated (.csv file)
                currentItem = items(i).Split(CChar(","))

                'Creates a new item from the file data and adds it to the itemdex
                itemdex.Add(New Item(currentItem))

            Next

        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading itemdex ""items.csv"" file. " + ex.Message)

            'Sets game to exit
            gameState = "Exit"

        End Try

    End Sub


    'Loads move file data into movedex
    Sub LoadMoves(ByVal fileLocation As String)

        Try

            'Loads move file
            Dim moveFile As New IO.StreamReader(fileLocation)

            'List to store file lines
            Dim moves As New List(Of String)

            'Stores current move
            Dim currentMove() As String

            'Loops until end of file
            While Not moveFile.EndOfStream

                'Adds file line to list
                moves.Add(moveFile.ReadLine)

            End While

            'Loops for each move in the file line list, excpet the first line which are titles
            For i = 1 To moves.Count - 1

                'Splits the current file line by commas as file is comma deliminated (.csv file)
                currentMove = moves(i).Split(CChar(","))

                'Creates a new move from the file data and adds it to the movedex
                movedex.Add(New Move(currentMove))

            Next

            'Close file as it is no longer required
            moveFile.Close()

        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading movedex ""Move index.csv"" file. " + ex.Message)

            'Sets game to exit
            gameState = "Exit"

        End Try

    End Sub


    'Loads pokemon file data into pokedex
    Sub LoadPokemon(ByVal fileLocation As String)

        Try

            'Loads pokemon file
            Dim pokemonfile As New IO.StreamReader(fileLocation)

            'List to store file lines
            Dim pokemons As New List(Of String)

            'Stores current pokemon
            Dim currentPokemon() As String

            'Loops until end of file
            While Not pokemonfile.EndOfStream

                'Adds file line to list
                pokemons.Add(pokemonfile.ReadLine)

            End While

            'Loops for each move in the file line list, excpet the first line which are titles
            For i = 1 To pokemons.Count - 1

                'Splits the current file line by commas as file is comma deliminated (.csv file)
                currentPokemon = pokemons(i).Split(CChar(","))

                'Creates a new pokemon from the file data and adds it to the game1.pokedex
                game1.pokedex.Add(New Pokemon(currentPokemon(0), CInt(currentPokemon(1)), CInt(currentPokemon(2)), New Move(movedex(CInt(currentPokemon(3))).getMoveAsString), New Move(movedex(CInt(currentPokemon(4))).getMoveAsString), New Move(movedex(CInt(currentPokemon(5))).getMoveAsString), New Move(movedex(CInt(currentPokemon(6))).getMoveAsString), currentPokemon(7), CInt(currentPokemon(8)), CInt(currentPokemon(9)), CInt(currentPokemon(10)), currentPokemon(11), CInt(currentPokemon(12))))

            Next

            'Close file as it is no longer required
            pokemonfile.Close()

        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading game1.pokedex ""pokemon.csv"" file. " + ex.Message)

            'Sets game to exit
            gameState = "Exit"

        End Try

    End Sub


    ''' <summary>
    ''' LoadContent will be called once per game and is the place to load
    ''' all of your content.
    ''' </summary>
    Protected Overrides Sub LoadContent()
        ' Create a new SpriteBatch, which can be used to draw textures.
        TileTextures = New SpriteBatch(GraphicsDevice)
        ' TODO: use Me.Content to load your game content here
    End Sub

    ''' <summary>
    ''' UnloadContent will be called once per game and is the place to unload
    ''' all content.
    ''' </summary>
    Protected Overrides Sub UnloadContent()
        ' TODO: Unload any non ContentManager content here
    End Sub

    ''' <summary>
    ''' Allows the game to run logic such as updating the world,
    ''' checking for collisions, gathering input, and playing audio.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    Protected Overrides Sub Update(ByVal gameTime As GameTime)

        'Sets game to refresh at 60Hz
        Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 60.0F)

        'If a battle or encounter has started play battle music
        If (gameState = "Battle" Or gameState = "Encounter") And playingSong = False Then

            'Denotes a song is playing
            playingSong = True

            'Stop current song
            MediaPlayer.Stop()

            Try

                'Loads battle song
                currentSong = Content.Load(Of Song)(battleSongLocation)

                'Plays battle song
                MediaPlayer.Play(currentSong)

                'Sets battle song to loop
                MediaPlayer.IsRepeating = True

            Catch ex As Exception

                'Displays error message
                MsgBox("Error loading battleSong. " + ex.Message)

            End Try

        End If

        'If the player is in the map then play the idle song
        If Not (gameState = "Battle" Or gameState = "Encounter" Or gameState = "MainMenu" Or gameState = "LoadGame" Or gameState = "NewGame") And playingSong = False Then

            'Denotes a song is playing
            playingSong = True

            'Stops current song
            MediaPlayer.Stop()

            Try

                'Loads idle song
                currentSong = Content.Load(Of Song)(idleSongLocation)

                'Plays idle song
                MediaPlayer.Play(currentSong)

                'Sets idle song to  loop
                MediaPlayer.IsRepeating = True

            Catch ex As Exception

                'Displays error message
                MsgBox("Error loading idleSong. " + ex.Message)

            End Try

        End If

        'If in main menu then play main menu song
        If gameState = "MainMenu" And playingSong = False Then

            'Denotes a song is playing
            playingSong = True

            'Stops current song
            MediaPlayer.Stop()

            Try

                'Loads main menu song
                currentSong = Content.Load(Of Song)(mainMenuSongLocation)

                'Plays main menu song
                MediaPlayer.Play(currentSong)

                'Sets main menu song to loop
                MediaPlayer.IsRepeating = True

            Catch ex As Exception

                'Displays error message
                MsgBox("Error loading Main menu song. " + ex.Message)

            End Try

        End If

        Select Case gameState

            Case "MainMenu"

                'Updates main menu logic
                MainMenuUpdate()

            Case "Playing"

                'Updates player logic
                player.update()

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

            Case "Sign"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "Chest"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "Menu"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "DisplayPokemon"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "PokemonInfo"

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "DisplayBag"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates map logic
                currentMap.Update(player.givex, player.givey, gameTime)

                'Updates messageBox logic
                messageBox.update()

            Case "Battle"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates battle logic
                gameBattle.Update(player, currentMap.getEntity(player.givex, player.givey), TileTextures)

            Case "Encounter"

                'Sets game to refresh at 10Hz
                Me.TargetElapsedTime = TimeSpan.FromSeconds(1.0F / 10.0F)

                'Updates battle logic
                gameBattle.Update(player, encounterPokemon, TileTextures)

            Case "Save"

                'Saves game
                saveGame()

            Case "Exit"

                'Exits game
                Me.Exit()

            Case "NewGame"

                'Creates new game
                newGame()

            Case "LoadGame"

                'Loads a game
                loadGame()

        End Select

        MyBase.Update(gameTime)

    End Sub 'Updates game logic


    'Saves a game
    Sub saveGame()

        'Creates a new file dialog
        Dim openFileDialog1 As OpenFileDialog = New OpenFileDialog()

        'Stores the file dialog result
        Dim result As DialogResult

        'Instructions for user as file dialog title
        openFileDialog1.Title = "Please select a .txt save file location"

        'Default file extension
        openFileDialog1.DefaultExt = "txt"

        'Allows user to select only one file
        openFileDialog1.Multiselect = False

        'Only displays .txt files
        openFileDialog1.Filter = "Text files (*.txt)|*.txt"

        'Loops until the file is a .txt file
        Do

            'Opens the file dialog and gets the file dialog result
            result = openFileDialog1.ShowDialog

        Loop Until System.IO.Path.GetExtension(openFileDialog1.FileName.ToString) = ".txt"

        'If the file dialog has been closed properly then the next code will execute
        If result = DialogResult.OK Then

            Try

                'Opens save file location
                Dim saveFile As New System.IO.StreamWriter(openFileDialog1.FileName.ToString, False)

                'Writes the map width height and texture location for the tiles
                saveFile.WriteLine(currentMap.getSaveConstAsString)

                'Double nested for loop, loops through the maze writing the current cells symbol, semi colon deliminated
                For c = 1 To currentMap.getSaveMazeList.Count

                    If c Mod currentMap.getWidth = 0 And c > 0 Then

                        'When it gets to a new row in the maze it starts a new line in the file
                        saveFile.WriteLine(currentMap.getSaveMazeList(c - 1))

                    Else

                        saveFile.Write(currentMap.getSaveMazeList(c - 1))

                    End If

                Next

                'Writes a blank line to the file to seperate blocks
                saveFile.WriteLine()

                'Double nested for loop, loops through the entitmap writing to file Entity (NPC/CHEST/SIGN) save date, semi colon deliminated
                For e = 1 To currentMap.getEntityMapList.Count

                    If e Mod currentMap.getWidth = 0 And e > 0 Then

                        'When it gets to a new row in the entitymap it starts a new line in the file
                        saveFile.WriteLine(currentMap.getEntityMapList(e - 1))

                    Else

                        saveFile.Write(currentMap.getEntityMapList(e - 1))

                    End If

                Next

                'Writes a blank line to the file to seperate blocks
                saveFile.WriteLine()

                'Writes the player's save data
                saveFile.WriteLine(player.getSaveString)

                'Closes the file as it is no longer required
                saveFile.Close()

            Catch ex As Exception


                MsgBox("Error saving file. " + ex.Message)

            End Try

            'Sets the gamestate back to playing so the user can continue to play
            gameState = "Playing"

        End If

    End Sub


    'Loads game
    Sub loadGame()

        'Creates a new file dialog
        Dim openFileDialog1 As OpenFileDialog = New OpenFileDialog()

        'Stores the file dialog result
        Dim result As DialogResult

        'Instructions for user as file dialog title
        openFileDialog1.Title = "Please select a .txt load file location"

        'Default file extension
        openFileDialog1.DefaultExt = "txt"

        'Allows user to select only one file
        openFileDialog1.Multiselect = False

        'Only displays .txt files
        openFileDialog1.Filter = "Text files (*.txt)|*.txt"

        'Loops until the file is a .txt file
        Do
            'Opens the file dialog and gets the file dialog result
            result = openFileDialog1.ShowDialog()

        Loop Until System.IO.Path.GetExtension(openFileDialog1.FileName.ToString) = ".txt"

        'If the file dialog has been closed properly then the next code will execute
        If result = DialogResult.OK Then

            Try

                'Loads the save file
                Dim saveFile As New System.IO.StreamReader(openFileDialog1.FileName.ToString)

                'Gets the map settings, width height and tile texture file location
                Dim mapSettings() As String = (Convert.ToString(saveFile.ReadLine)).Split(CChar(";"))

                'closes save file as it is no longer required
                saveFile.Close()

                'Creates a new map
                currentMap = New Map(CByte(mapSettings(0)), CByte(mapSettings(1)), True, openFileDialog1.FileName.ToString)

                'Sets the game state to "Playing" so the user can play the game
                gameState = "Playing"

            Catch ex As Exception

                'Displays error message
                MsgBox("Error loading file, new game started instead " + ex.Message)

                'Takes user back to main menu if there was an error loading the save file
                gameState = "MainMenu"

            End Try

        End If

    End Sub


    'Creates a new game, by setting the game1 attributes to new values
    Sub newGame()

        'Creates a new map
        currentMap = New Map(MapWidth, MapHeight, False, "")

        'Populates map with NPC - NPC/CHEST/SIGN
        currentMap.populateMapWithNPC(game1.pokedex, itemdex)

        'Creates a new player
        player = New Player

        'Stores the current pokemons position in the game1.pokedex
        Dim randomPokemonPosition As Integer

        'Loops 6 times - the max number of pokemon a player can have
        For i = 0 To 5

            'Selects a random number between 0 and the last pokemon ID
            randomPokemonPosition = CInt(Math.Floor(Rnd() * game1.pokedex.Count))

            'Gives the player a random pokemon
            player.getpokemon(New Pokemon(game1.pokedex(randomPokemonPosition).giveName, game1.pokedex(randomPokemonPosition).giveLevel, game1.pokedex(randomPokemonPosition).giveMaxHP, game1.pokedex(randomPokemonPosition).giveMoves(0), game1.pokedex(randomPokemonPosition).giveMoves(1), game1.pokedex(randomPokemonPosition).giveMoves(2), game1.pokedex(randomPokemonPosition).giveMoves(3), game1.pokedex(randomPokemonPosition).giveType, game1.pokedex(randomPokemonPosition).giveAttack, game1.pokedex(randomPokemonPosition).giveDefence, game1.pokedex(randomPokemonPosition).giveSpeed, game1.pokedex(randomPokemonPosition).giveTextureAsString, game1.pokedex(randomPokemonPosition).giveID))

        Next

        'Loops 6 times -  the max number of items a player can have
        For i = 0 To 5

            'Gives the player a random item
            player.getitem(itemdex(CInt(Math.Floor(Rnd() * (itemdex.Count)))))

        Next

        'Sets the gameState so the user can play the game
        gameState = "Playing"

    End Sub
    ''' <summary>
    ''' This is called when the game should draw itself.
    ''' </summary>
    ''' <param name="gameTime">Provides a snapshot of timing values.</param>
    ''' 


    Protected Overrides Sub Draw(ByVal gameTime As GameTime) 'Draws the game

        'Sets game screen background to a light blue
        GraphicsDevice.Clear(Color.CornflowerBlue)

        Select Case gameState

            Case "MainMenu"

                'Draws main menu
                MainMenuDraw()

            Case "Playing"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

            Case "Sign"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws sign
                messageBox.draw(TileTextures)

            Case "Chest"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws chest GUI
                messageBox.draw(TileTextures)

            Case "Menu"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws menu
                messageBox.draw(TileTextures)

            Case "DisplayPokemon"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws players pokemon list GUI
                messageBox.draw(TileTextures)

            Case "PokemonInfo"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws game1.pokedex entry
                messageBox.draw(TileTextures)

            Case "DisplayBag"

                'Draws the map
                currentMap.drawAll(TileTextures, MapWidth, MapHeight, player.givex, player.givey)

                'Draws the player
                player.drawAll(TileTextures)

                'Draws players bag GUI
                messageBox.draw(TileTextures)

            Case "Battle"

                'Draws the battle
                gameBattle.Draw(TileTextures, player, currentMap.getEntity(player.givex, player.givey))

            Case "Encounter"

                'Draws the encounter
                gameBattle.Draw(TileTextures, player, encounterPokemon)

        End Select

        MyBase.Draw(gameTime)

    End Sub


    'Draws the main menu
    Sub MainMenuDraw()

        Dim _mainMenuTexture As Texture2D

        Dim destinationRectangle, sourceRectangle As Rectangle

        'The main munu size
        Const sizeInPixels As Integer = 512

        Try

            'Loads the main menu texture
            _mainMenuTexture = Game1.ContentLoader.Load(Of Texture2D)("start menu")

            destinationRectangle = New Rectangle(0, 0, sizeInPixels, sizeInPixels)

            sourceRectangle = New Rectangle(0, 0, sizeInPixels, sizeInPixels)

            TileTextures.Begin()

            'Draws main menu texture
            TileTextures.Draw(_mainMenuTexture, sourceRectangle, destinationRectangle, Color.White)

            TileTextures.End()

        Catch ex As Exception

            'Displays the menu options if an error occurs
            MsgBox("Press 1 for Exit, Press 2 for Load game, Press 3 to start a new game")

        End Try

    End Sub


    'Updates MainMenu logic
    Sub MainMenuUpdate()

        'Gets keyboard state
        kbState = Keyboard.GetState

        'If number 1 is pressed the exit game
        If kbState.IsKeyDown(Keys.D1) Then

            'Denotes a song isn't playing
            playingSong = False

            'Stops playing the main menu song
            MediaPlayer.Stop()

            'Plays selection sound effect
            pressA.Play()

            'Sets gameState to exit
            gameState = "Exit"

            'If number 2 is pressed a game will be loaded
        ElseIf kbState.IsKeyDown(Keys.D2) Then

            'Denotes a song isn't playing
            playingSong = False

            'Stops playing the main menu song
            MediaPlayer.Stop()

            'Plays selection sound effect
            pressA.Play()

            'Sets gameState to Load game
            gameState = "LoadGame"

            'If number 2 is pressed a game will be loaded
        ElseIf kbState.IsKeyDown(Keys.D3) Then

            'Denotes a song isn't playing
            playingSong = False

            'Stops playing the main menu song
            MediaPlayer.Stop()

            'Plays selection sound effect
            pressA.Play()

            'Plays selection sound effect
            gameState = "NewGame"

        End If

    End Sub


    'Converts a string to a list for the log
    Public Shared Function convertToList(ByVal newText As String) As List(Of String)

        'The maximum number of characters on one line
        Dim charLimit As Integer = CInt(Math.Floor(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth + Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight) / 2) * (48 / 512)))

        'Stores how many characters are currently on a line
        Dim counter As Integer = 0

        'Stores the current character in the text
        Dim inTextPosition As Integer = 1

        'Stores temporarily the current line of text
        Dim tempText As String = ""

        'Stores the string text as a list of text
        Dim Text As New List(Of String)

        'Loops until it is at the end of the string text
        Do

            'Resets the counter
            counter = 0

            'Resets temporay text
            tempText = ""

            Do 'Loops until its at the end of a line, at the character limit

                'Adds the current character in the string text to the curent line of text, tempText
                tempText = tempText & newText(inTextPosition - 1)

                'Increments counter
                counter += 1

                'Increments inText position
                inTextPosition += 1

            Loop Until counter > charLimit - 1 Or inTextPosition = newText.Length

            'Adds the line of text to the list of text
            Text.Add(tempText)

        Loop Until inTextPosition = newText.Length

        Return Text

    End Function


End Class
