'A class to store battles
Public Class Battle

    'Keep count of enemy pokemon which have fainted (hp = 0)
    Private enemyFainted As Integer

    'Keeps count of player pokemon whcih have fainted
    Private playerFainted As Integer

    'Boolean value to dtermine if the battle is an encounter or not, if true then player can catch pokemon else they can't
    Private isEncounter As Boolean

    'Stores the type effectivness as a dictionary stored as key defender then attacker and the definition being the effectiveness multiplier
    Private effectiveness As New Dictionary(Of String, Decimal)

    'Stores a list of effects that moves can do
    Private effects As New Dictionary(Of Integer, String)

    'Stores the current player pokemon, which is in the battlefield, position in the player pokemon list
    Private CurrentPlayerPokemonInt As Integer

    'Stores the current enemy pokemon, which is in the battlefield, position in the enemy pokemon list
    Private CurrentEnemyPokemonInt As Integer

    'Stores the current state of the battle
    Private battleState As String

    'Stores the text which will be in the pop up text box
    Private logText As String

    'Stores the pointer position in the battle menus
    Private pointerPosition As Vector2

    'Boolean to decide if player/enemy is switching pokemon because pokemon fainted
    Private switchingFaintedPokemon As Boolean


    'Creates a new battle
    Sub New(ByRef player1 As Player, ByRef enemy As Player, ByVal isEncounterBoolean As Boolean)

        'Stops playing the current song, idleSong
        Game1.playingSong = False

        'Defaults the boolean to false as the player/enemy will be sending out pokemon first
        switchingFaintedPokemon = False


        playerFainted = 0


        enemyFainted = 0

        'Goes through the player pokemon list adds 1 for all the pokemon which are fainted to the playerFainted count
        For i = 0 To player1.getPokemonList.Count - 1

            If player1.getPokemonList(i).giveHP <= 0 Then

                playerFainted += 1

            End If

        Next

        'Goes through the enemy pokemon list adds 1 for all the pokemon which are fainted to the enemyFainted count
        For j = 0 To enemy.getPokemonList.Count - 1

            If enemy.getPokemonList(j).giveHP <= 0 Then

                enemyFainted += 1

            End If

        Next

        'Defaults  the pointer position
        pointerPosition = New Vector2(0, 0)

        'Sets the encounter boolean from the passed boolean variable
        isEncounter = isEncounterBoolean

        'Loads effectiveness dictionary
        loadDictionary("content/types.csv")

        'Loads the effects dictionary
        loadEffects("content/effects.csv")

        'Sets the battle state to begin to start the battle
        battleState = "begin"

    End Sub


    'Loads the effectivness file into the dicitonary
    Sub loadDictionary(ByVal filePath As String)

        'Stores all the file lines
        Dim myList As New List(Of String)

        'Stores the current line of the file which is being used to add to the dictionary
        Dim Typelist() As String

        'Stores all the type names
        Dim typename As New List(Of String)


        Try

            'Loads the type effectiveness table
            Dim file As New IO.StreamReader(filePath)

            'Loops until it gets to the end of the file
            While file.EndOfStream = False

                'Adds each line of the file to a list
                myList.Add(Convert.ToString(file.ReadLine))

            End While

            'Closes the file as it is no longer required
            file.Close()

            'Selects the first item in the list as splits it to get all type names
            Typelist = myList(0).Split(CChar(","))

            'Loops from 1, as the first type is blank and is not required, to the last item
            For i = 1 To UBound(Typelist)

                'Adds the type name to a type list
                typename.Add(Typelist(i))

            Next

            'Double nested for loop, loops for the number of type names squared to match up every type
            For i = 1 To typename.Count

                'Splits the current item in the list, which stores the file lines, by commas as the file is commma deliminated
                Typelist = myList(i).Split(CChar(","))

                'Loops through all the type names for the defending type name, the attack type name is the first item in the file line
                For j = 1 To typename.Count

                    'Adds the the dictionary, "defending type name" + "attacking type name" as the key and the effectiveness multipler as the item
                    effectiveness.Add(CStr(typename(j - 1)) + CStr(Typelist(0)), CDec(Typelist(j)))

                Next

            Next

            'Catches any exceptions whihc would cause a crash e.g. missing file or adding an existing key to the dictionary
        Catch ex As Exception

            'Displays the error message
            MsgBox("Error loading type file ""types.csv"". " + ex.Message + " Battle will exit")

            'Exits battle to prevent any furhter crashes, by setting the game state to playing, thus ending the battle
            Game1.gameState = "Playing"

        End Try

    End Sub


    'Loads the effect file into the effect dictionary
    Sub loadEffects(ByVal filePath As String)

        'Stores all the lines of the file
        Dim fileList As New List(Of String)

        'Stores the current line of the file whcih is being processed
        Dim currentEffect() As String


        Try

            'Loads the effects file
            Dim file As New IO.StreamReader(filePath)

            'Loops until the end of the file
            While file.EndOfStream = False

                'Adds the current line of the file to the list
                fileList.Add(Convert.ToString(file.ReadLine))

            End While

            'Loops from 1, as the first line is the titles of the columns whihc is not required, to the end of the list
            For i = 1 To fileList.Count - 1

                'Splits the current item in the list by colons as the file is colon deliminated
                currentEffect = fileList(i).Split(CChar(":"))

                'Adds the effect id as the key and the effect as the item to the dictionary
                effects.Add(CInt(currentEffect(0)), currentEffect(1))

            Next

            'Catches any exceptions which would cause a crash e.g. missing file or adding an existing key to the dictionary
        Catch ex As Exception

            'Displays the error message
            MsgBox("Error loading effects file ""effects.csv"". " + ex.Message + " Battle will exit")

            'Exits battle to prevent any furhter crashes, by setting the game state to playing, thus ending the battle
            Game1.gameState = "Playing"

        End Try

    End Sub


    'Updates the battle logic
    Sub Update(ByRef player1 As Player, ByRef enemy As Player, ByRef Tiles As SpriteBatch)

        'Gets the current keyboard state for any battle inputs
        Game1.kbState = Keyboard.GetState

        'Variable to store the players chosen move
        Dim selectedMove As Move

        'Variable to store the players chosen item
        Dim selectedItem As Item

        'Check if all pokemon fainted if all fainted then game state = exit, display winner, then gamestate  = playing
        If playerFainted = player1.getPokemonList.Count Or enemyFainted = enemy.getPokemonList.Count Or enemy.getPokemonList.Count = 0 Then

            'If all the player pokemon are fainted then the enemy wins
            If playerFainted = player1.getPokemonList.Count Then

                logText = " Enemy wins !!! press E to exit."

                battleState = "exit"

                'If all the enemy pokemon have fainted then the player wins
            ElseIf enemyFainted = enemy.getPokemonList.Count Then

                logText = "Player wins !!! press E to exit"

                battleState = "exit"

            End If

        End If

        'Check if current pokemons fainted if so switch
        'Will only check if it is no the end of the battle
        If (Not battleState = "exit") Then

            'Will excute next code if the player is not already trying to swtich pokemon and the current player pokemon is fainted
            If player1.getPokemonList(CurrentPlayerPokemonInt).giveHP <= 0 And switchingFaintedPokemon = False Then

                'Increments the number of player fainted pokemon by 1
                playerFainted = playerFainted + 1

                'If all the player pokemon are fainted then they won't switch
                If playerFainted <> player1.getPokemonList.Count Then

                    switchingFaintedPokemon = True

                    battleState = "pokemons"

                End If

            End If

            'Will excute next code if the enemy is not already trying to swtich pokemon and the current player enemy is fainted
            If enemy.getPokemonList(CurrentEnemyPokemonInt).giveHP <= 0 Then

                'Increments the number of enemy fainted pokemon by 1
                enemyFainted = enemyFainted + 1


                'If all the enemy pokemon are fainted then they won't switch
                If enemyFainted <> enemy.getPokemonList.Count Then

                    switchAI(player1, enemy)

                End If

            End If

        End If

        'A select case to decide what will happen next in the battle dependent on the current battleState
        Select Case battleState

            Case "begin"

                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    battleState = "waiting"

                End If

            Case "waiting"

                If Game1.kbState.IsKeyDown(Keys.E) Then

                    battleState = "options"

                End If

            Case "displayLog"

                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    logText = ""

                    battleState = "waiting"

                End If

            Case "options"

                'Hardcoded 3 options "move", "items" and "pokemons"
                SetPointerY(3)

                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    Select Case pointerPosition.Y

                        Case 0

                            battleState = "moves"

                        Case 1

                            battleState = "pokemons"

                        Case 2

                            battleState = "items"

                    End Select

                ElseIf Game1.kbState.IsKeyDown(Keys.R) Then

                    'Stops the battle song
                    Game1.playingSong = False

                    'Sets the game state to playing to exit the battle
                    Game1.gameState = "Playing"

                    Game1.gameState = "Playing"

                End If

            Case "pokemons"

                'Number of positions = number of pokemon in player pokemon list
                SetPointerY(player1.getPokemonNamesAndHP.Count)

                'Will excute if the player is not being forced to switch
                If Game1.kbState.IsKeyDown(Keys.E) And switchingFaintedPokemon = False Then

                    battleState = "options"

                End If

                'Selects the current pokemon at the pointer position
                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    'Only swaps pokemon if the pokemon isn't fainted or not the current pokemon
                    If player1.getPokemonList(CInt(pointerPosition.Y)).giveHP > 0 And pointerPosition.Y <> CurrentPlayerPokemonInt Then

                        battleState = "doSwitch"

                    End If

                End If

            Case "moves"

                'Number of positions = number of moves pokemon has
                SetPointerY(player1.getPokemonList(CurrentPlayerPokemonInt).getMovesAndPPAsString.Count)

                If Game1.kbState.IsKeyDown(Keys.E) Then

                    battleState = "options"

                End If

                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    'Selects the move at the pointer position id the move has more than 0 uses left
                    If player1.getPokemonList(CurrentPlayerPokemonInt).giveMoves(CInt(pointerPosition.Y)).givePP > 0 Then

                        selectedMove = player1.getPokemonList(CurrentPlayerPokemonInt).giveMoves(CInt(pointerPosition.Y))

                        battleState = "useMove"

                    End If

                End If

            Case "items"

                'Number of positions = the number of items the player has in there bag
                SetPointerY(player1.getItemNames.Count)

                If Game1.kbState.IsKeyDown(Keys.E) Then

                    battleState = "options"

                End If

                'Selects the item at the pointer position to use
                If Game1.kbState.IsKeyDown(Keys.Enter) Then

                    selectedItem = player1.getBag(CInt(pointerPosition.Y))

                    'Removes the item so it can't be re used
                    player1.removeItem(CInt(pointerPosition.Y))

                    battleState = "useItem"

                End If

            Case "exit"

                If Game1.kbState.IsKeyDown(Keys.E) Then

                    'Stops the battle song
                    Game1.playingSong = False

                    'Sets the game state to playing to exit the battle
                    Game1.gameState = "Playing"

                End If

        End Select

        Select Case battleState

            Case "doSwitch"

                'The enemy makes it move
                battleAI(player1, enemy)

                'Sets the current Player pokemon to the selected pokemon at the pointer position if the selected pokemon isnt Fainted
                If player1.getPokemonList(CInt(pointerPosition.Y)).giveHP > 0 Then

                    'Set to false as the switch is finished
                    switchingFaintedPokemon = False

                    CurrentPlayerPokemonInt = CInt(pointerPosition.Y)

                    'Adds which pokemon the player sent out
                    logText = logText + " " + player1.getName + "  sent out " + player1.getPokemonList(CurrentPlayerPokemonInt).giveName + " "

                    battleState = "displayLog"

                End If

            Case "useMove"

                'If the player pokemon is faster then it will go first, otherwise the enemy will go first
                If player1.getPokemonList(CurrentPlayerPokemonInt).giveSpeed > enemy.getPokemonList(CurrentEnemyPokemonInt).giveSpeed Then

                    move(player1, enemy, selectedMove)

                    battleAI(player1, enemy)

                Else

                    battleAI(player1, enemy)

                    move(player1, enemy, selectedMove)

                End If

                'Sets the battle state to display log to show what moves are used and how much damage they did
                battleState = "displayLog"

            Case "useItem"

                'If the selected item is a *ball eg. a pokeball then the next code will execute
                Select Case selectedItem.getName.Contains("ball")

                    Case True

                        'The player can only use pokeballs in encounters
                        If isEncounter = True Then

                            'Stores a random number
                            Dim randomSuccessChance As Decimal = CDec(Rnd())

                            'If the random chance is less than 0.75 then the player will catch the pokemon, effectively giving the player a 3/4 to catch the pokemon
                            If randomSuccessChance <= 0.75 And player1.getPokemonList.Count < 6 Then

                                'Logs the successful capture
                                logText = logText + " You caught that pokemon. "

                                'Gives the player the pokemon
                                player1.getpokemon(New Pokemon(enemy.getPokemonList(CurrentEnemyPokemonInt).giveName, enemy.getPokemonList(CurrentEnemyPokemonInt).giveLevel, enemy.getPokemonList(CurrentEnemyPokemonInt).giveMaxHP, enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(0), enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(1), enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(2), enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(3), enemy.getPokemonList(CurrentEnemyPokemonInt).giveType, enemy.getPokemonList(CurrentEnemyPokemonInt).giveAttack, enemy.getPokemonList(CurrentEnemyPokemonInt).giveDefence, enemy.getPokemonList(CurrentEnemyPokemonInt).giveSpeed, enemy.getPokemonList(CurrentEnemyPokemonInt).giveTextureAsString, enemy.getPokemonList(CurrentEnemyPokemonInt).giveID))

                                'Removes the pokemon from the enemy
                                enemy.removePokemon(CurrentEnemyPokemonInt)

                            Else

                                'Logs the unsucessful capture
                                logText = logText + " You failed to catch that pokemon "

                            End If

                        Else

                            'Logs that the player can't use pokeballs in NPC battles
                            logText = logText + " You can't use a pokeball in battle "

                        End If

                        'Executes the next code if the selected itme isn't a pokeball
                    Case False

                        'Applies the item effect to the Player Pokemon
                        processEffect(player1, enemy, "PS", selectedItem.getStat, selectedItem.getValue)

                        'Logs the item effect
                        logText = logText + " " + player1.getName + "  used " + selectedItem.getName + " "
                End Select

                'Enemy makes their move
                battleAI(player1, enemy)

                battleState = "displayLog"

        End Select

    End Sub


    'Draws the battle
    Sub Draw(ByRef battleTexture As SpriteBatch, ByRef player1 As Player, ByRef enemy As Player)

        'Stores background texture
        Dim _backgroundTexture As Texture2D

        'Where on the texture sheet the texture begins
        Dim backgroundTexturePosition As Vector2 = New Vector2(285, 301)

        'How big the texture is in pixels
        Dim backgroundTextureSize As Vector2 = New Vector2(256, 144)

        'Where the texture will be draw on the screen
        Dim backgroundScreenLocation As Vector2 = New Vector2(0, 0)

        'How big the texture will be on the screen
        Dim backgroundSize As Integer = CInt(Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth)

        'Where on the texture sheet the texture begins
        Dim foregroundTexturePosition As Vector2 = New Vector2(1396, 1467)

        'How big the texture is in pixels
        Dim foregroundTextureSize As Vector2 = New Vector2(256, 72)

        'Where the texture will be draw on the screen
        Dim foregroundScreenLocation As Vector2 = New Vector2(0, 260)

        Const _backgroundTextureLocation As String = "battle"

        Dim sourceRectange, destinationRectangle As Rectangle

        'Tries loading texture, but will catch errors ie. if the file has been deleted
        Try

            battleTexture.Begin()

            'Loads the texture for the background and foreground of the battle
            _backgroundTexture = Game1.ContentLoader.Load(Of Texture2D)(_backgroundTextureLocation)

            'Source rectanlge from predtermined consts
            sourceRectange = New Rectangle(CInt(backgroundTexturePosition.X), CInt(backgroundTexturePosition.Y), CInt(backgroundTextureSize.X), CInt(backgroundTextureSize.Y))

            'Background texture will fill the game screen
            destinationRectangle = New Rectangle(CInt(backgroundScreenLocation.X), CInt(backgroundScreenLocation.Y), backgroundSize, backgroundSize)

            'Draws background
            battleTexture.Draw(_backgroundTexture, destinationRectangle, sourceRectange, Color.White)

            'Source recatangle from predetermined consts
            sourceRectange = New Rectangle(CInt(foregroundTexturePosition.X), CInt(foregroundTexturePosition.Y), CInt(foregroundTextureSize.X), CInt(foregroundTextureSize.Y))

            destinationRectangle = New Rectangle(CInt(foregroundScreenLocation.X), CInt(foregroundScreenLocation.Y), backgroundSize, CInt(backgroundSize / 2))

            'Draws foreground - pokemon spots
            battleTexture.Draw(_backgroundTexture, destinationRectangle, sourceRectange, Color.White)

            battleTexture.End()

        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading Background texture. " + ex.Message)

            Game1.gameState = "Playing"

        End Try

        'Will execute next code when it is not the begining of battle, as at the start player/enemy pokemon are unknown to player visually
        If Not battleState = "begin" And player1.getPokemonList.Count > 0 And enemy.getPokemonList.Count > 0 Then

            'Draw player pokemon
            drawPokemon(player1.getPokemonList(CurrentPlayerPokemonInt), 0, 300, battleTexture)

            'Draw enemy pokemon
            drawPokemon(enemy.getPokemonList(CurrentEnemyPokemonInt), 280, 160, battleTexture)

            'Draw player health bar
            drawBattleHealthBar(battleTexture, player1.getPokemonList(CurrentPlayerPokemonInt).giveName, CStr(player1.getPokemonList(CurrentPlayerPokemonInt).giveHP), 0, 0)

            ' Draw enemy heallth bar
            drawBattleHealthBar(battleTexture, enemy.getPokemonList(CurrentEnemyPokemonInt).giveName, CStr(enemy.getPokemonList(CurrentEnemyPokemonInt).giveHP), 0, 30)

        End If

        'At the start of the battle it will display which pokemon have been sent out
        If battleState = "begin" And Game1.player.getPokemonList.Count > 0 Then

            drawBattleTextLog(battleTexture, Game1.convertToList("Player Sent out " + player1.getPokemonList(CurrentPlayerPokemonInt).giveName + " with " + CStr(player1.getPokemonList(CurrentPlayerPokemonInt).giveHP) + "HP. Enemy sent out " + enemy.getPokemonList(CurrentEnemyPokemonInt).giveName + " with " + CStr(enemy.getPokemonList(CurrentEnemyPokemonInt).giveHP) + " HP."))

        End If

        'Will display the log, showing what happend in each player/enemy move
        If battleState = "displayLog" Or battleState = "exit" Then

            drawBattleTextLog(battleTexture, Game1.convertToList(logText))

        End If

        Select Case battleState

            Case "options"

                'Will display menu options
                drawSelectOption(battleTexture, New List(Of String) From {"Move", "Switch", "Item"})

            Case "pokemons"

                'Will display pokemons player has
                drawSelectOption(battleTexture, player1.getPokemonNamesAndHP)

            Case "moves"

                'displays the moves the current player pokemon has
                drawSelectOption(battleTexture, player1.getPokemonList(CurrentPlayerPokemonInt).getMovesAndPPAsString)

            Case "items"

                'displays the current items the player has
                drawSelectOption(battleTexture, player1.getItemNames)

        End Select

    End Sub


    'Draws a pokemon, given the pokemon, the X poisition and Y position the texture will be drawn for
    Sub drawPokemon(ByVal pokemon As Pokemon, ByVal xDestination As Integer, ByVal yDestination As Integer, ByVal pokemonSpritebatch As SpriteBatch)

        'Stores the pokemons texture
        Dim pokemonSpriteTexture As Texture2D

        Const pokemonDrawSpriteSize As Integer = 200

        Try

            'Loads the pokemon texture
            pokemonSpriteTexture = Game1.ContentLoader.Load(Of Texture2D)(pokemon.getSpriteLocation)

            Dim sourceRectange, destinationRectangle As Rectangle

            'Source rectangle by default starts at the top left side of the texture, and will select a region from the pokemon attributes for the Texture width and height
            sourceRectange = New Rectangle(0, 0, pokemon.getSpriteWidth, pokemon.getSpriteHeight)

            'By default all pokemon will be drawn 200*200 pixels to fit in the battle
            destinationRectangle = New Rectangle(xDestination, yDestination, pokemonDrawSpriteSize, pokemonDrawSpriteSize)

            pokemonSpritebatch.Begin()

            'Draws the pokemon texture
            pokemonSpritebatch.Draw(pokemonSpriteTexture, destinationRectangle, sourceRectange, Color.White)

            pokemonSpritebatch.End()

        Catch ex As Exception

            MsgBox("Error drawing " + pokemon.giveName + " sprite. " + ex.Message)

        End Try

    End Sub


    'Draws the battleLog, given the log text
    Sub drawBattleTextLog(ByRef Tiles As SpriteBatch, ByRef text As List(Of String))

        Dim destinationRectangle, sourceRectange As Rectangle

        Try

            'Loads the textbox texture        
            Dim _textBoxTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("textbox")

            'Constants for the textbox texture width and height
            Const textBoxWidth As Integer = 250

            Const textBoxHeight As Integer = 44

            'How many times bigger the textbox texture will be than it actually is
            Const textBoxMultiplier As Decimal = CDec(1.5)

            'Textoff sets, for how far into the textbox the texture will be written
            Const textOffsetWidth As Integer = 10

            Const textOffsetHeight As Integer = 4

            'Makes a rectagle the size of the textbox texture
            sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

            'Draws the textbox at the centre of the screen - calculated from the current gmae window size divided by 2 then take away the textbox size, draws the text box its original size * the multiplyer
            destinationRectangle = New Rectangle(CInt((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2), CInt((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - ((textBoxHeight * textBoxMultiplier) / 2)), CInt(textBoxWidth * textBoxMultiplier), CInt(textBoxHeight * textBoxMultiplier + ((textBoxHeight * textBoxMultiplier) / 2)))

            Tiles.Begin()

            'Draws the textbox texure
            Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

            'For all the items of text in the text list it will draw the text
            For i = 0 To text.Count - 1

                'Starts the draw position of the text as the same place of the textbox but with the text offset, it moves down each line of text by 10 pixels
                Tiles.DrawString(Game1.font, text(i), New Vector2(CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (textBoxWidth * textBoxMultiplier) / 2) + (textOffsetWidth * textBoxMultiplier)), CInt(((Game1.graphics.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - ((textBoxHeight * textBoxMultiplier) / 2))) + (textOffsetHeight * textBoxMultiplier) + (i * 10)), Color.Black)

            Next

            Tiles.End()

        Catch ex As Exception

            MsgBox("Error loading textbox texture. " + ex.Message)

        End Try

    End Sub


    'Draws a health the a given pokemons name, and health in a textbox at a given X and Y position
    Sub drawBattleHealthBar(ByRef Tiles As SpriteBatch, ByRef name As String, ByRef health As String, ByVal destinationXposition As Integer, ByVal destinationYposition As Integer)

        Dim destinationRectangle, sourceRectange As Rectangle

        Try

            'Loads the textbox texture
            Dim _textBoxTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("textbox")

            'Constants for the textbox texture width and height
            Const textBoxWidth As Integer = 250

            Const textBoxHeight As Integer = 44

            'How many times bigger the textbox texture will be than it actually is
            Const textBoxMultiplier As Decimal = CDec(0.6)

            'Makes a rectagle the size of the textbox texture
            sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

            'Will destine the textbox texture at the given X and Y position, at the size of the textbox texure size multiplied by the multiplier
            destinationRectangle = New Rectangle(destinationXposition, destinationYposition, CInt(Math.Floor(textBoxWidth * textBoxMultiplier)), CInt(Math.Floor(textBoxHeight * textBoxMultiplier)))

            Tiles.Begin()

            'Draws the textbox texture
            Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

            'Draws the pokemons name
            Tiles.DrawString(Game1.font, name, New Vector2(destinationXposition + (CInt(Math.Floor(textBoxHeight * textBoxMultiplier))), destinationYposition), Color.Black)

            'Draws pokemons HP
            Tiles.DrawString(Game1.font, "HP: " + health, New Vector2(destinationXposition + (CInt(Math.Floor(textBoxHeight * textBoxMultiplier))), destinationYposition + 10), Color.Black)

            Tiles.End()

        Catch ex As Exception

            MsgBox("Error drawing Health box. " + ex.Message)

        End Try

    End Sub


    'Draws the options for any menu in a battle, given the list of options as a string
    Sub drawSelectOption(ByRef Tiles As SpriteBatch, ByRef Text As List(Of String))

        Dim destinationRectangle, sourceRectange As Rectangle

        'Loads the textbox texture
        Dim _textBoxTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("textbox")

        'Loads the pointer texture
        Dim _pointerTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("pointer")

        'Constants for the textbox texture width and height
        Const textBoxWidth As Integer = 250

        Const textBoxHeight As Integer = 44

        'Constants for the pointers starting position
        Const pointerHeight As Integer = 15

        Const pointerWidth As Integer = 19

        sourceRectange = New Rectangle(0, 0, textBoxWidth, textBoxHeight)

        'Predetermined rectangle for the position of the textbox texture
        destinationRectangle = New Rectangle(0, 60, 176, 200)

        Tiles.Begin()

        'Draws the textbox texture
        Tiles.Draw(_textBoxTexture, destinationRectangle, sourceRectange, Color.White)

        'Loops for all the options in the menu
        For i = 0 To Text.Count - 1

            'Draws the menu option at predetermined X position and Y but increase Y by 15 pixels for each menu option
            Tiles.DrawString(Game1.font, Text(i), New Vector2(30, 80 + i * 15), Color.Black)

        Next

        sourceRectange = New Rectangle(0, 0, pointerWidth, pointerHeight)

        'Draws the pointer at the same X position but different Y position depending on where the pointer logically is
        destinationRectangle = New Rectangle(0, CInt(80 + pointerPosition.Y * 15), pointerWidth, pointerHeight)

        'Draws the pointer
        Tiles.Draw(_pointerTexture, destinationRectangle, sourceRectange, Color.White)

        Tiles.End()

    End Sub


    'The logic to decide if the player move will succeed or not aswell as the effect
    Sub move(ByRef player1 As Player, ByRef enemy As Player, ByRef selectedMove As Move)

        'Decrease the players current pokemon selected moves PP so it can use the move one less time
        player1.getPokemonList(CurrentPlayerPokemonInt).giveMoves(CInt(pointerPosition.Y)).decreasePP()

        Dim accuracyRandom As Integer

        'Generates a random number
        accuracyRandom = CInt(Rnd())

        'If the random number is less than the move accuracy then it will execute the next code, effectively giving the move the chance to succeed as the same as its accuarcy
        If accuracyRandom <= selectedMove.GiveAccuracy Then

            'String to store the effect text
            Dim effectText As String = ""

            'If the effect isn't "No effect" then it will execute the next code
            If Not selectedMove.giveEffectID = 0 Then

                'Stores the effect information from the ID given by the move
                Dim moveEffect() As String = effects(selectedMove.giveEffectID).Split(CChar(","))

                'Effect text is set to the last part of the effect information
                effectText = moveEffect(5)

                'The effect will then get processed
                processEffect(player1, enemy, "P" + moveEffect(2), moveEffect(3), CDec(moveEffect(4)))

                'Else the effectText will be nothing
            Else

                effectText = ""

            End If

            'The damage of the move will be calulated
            Dim damageTaken As Integer = CInt(damageCalculation(player1.getPokemonList(CurrentPlayerPokemonInt).giveLevel, player1.getPokemonList(CurrentPlayerPokemonInt).giveAttack, enemy.getPokemonList(CurrentEnemyPokemonInt).giveDefence, selectedMove.Givetype, player1.getPokemonList(CurrentPlayerPokemonInt).giveType, enemy.getPokemonList(CurrentEnemyPokemonInt).giveType, selectedMove.GiveBaseAttack))

            'The enemy pokemon will take the damage, decreasing its HP
            enemy.getPokemonList(CurrentEnemyPokemonInt).takeDamage(damageTaken)

            'The move used and damage will be added to the log text
            logText = logText + "Player " + " " + player1.getPokemonList(CurrentPlayerPokemonInt).giveName + " used " + selectedMove.GiveName + " did " + CStr(damageTaken) + " points of damage " + effectText + " "

            'Else the move will fail
        Else

            'The move failure will be added to the log text
            logText = logText + " Player move failed. "

        End If

    End Sub


    'This will determine if the AI will chose a move, item or switch pokemon
    Sub battleAI(ByRef player1 As Player, ByRef enemy As Player)

        'Randomize the random seed
        Randomize()

        'Decimals to store decimal chances of a move, switch or item being picked
        Dim moveChance, switchChance, itemChance As Decimal

        If enemyFainted <> enemy.getPokemonList.Count And enemy.getPokemonList.Count > 0 Then

            'If the enemy pokemon HP is less than 50% and has a potion then it is more likely to choose to choose an item
            If enemy.haveItem("potion") = True And (enemy.getPokemonList(CurrentEnemyPokemonInt).giveHP / enemy.getPokemonList(CurrentEnemyPokemonInt).giveMaxHP) <= 0.5 Then

                itemChance = CDec(0.33)

                'if the enemy's bag is empty then it wont try to choose and item
            ElseIf enemy.getBag.Count = 0 Then

                itemChance = CDec(0)

            Else

                itemChance = CDec(0.1)

            End If

            'If the enemy's pokemons HP is less than 25% then it will be more likely to switch pokemon than choose a move or switch in a 3:1
            If (enemy.getPokemonList(CurrentEnemyPokemonInt).giveHP / enemy.getPokemonList(CurrentEnemyPokemonInt).giveMaxHP) <= 0.25 Then

                switchChance = CDec(0.75) * (1 - itemChance)

                moveChance = CDec(0.25) * (1 - itemChance)

                'If the current player pokemon is 1.5x effective or above than the  current enemy pokemon the n the AI will be more likely to
                'Switch than do a move in a 9:1 ratio
            ElseIf effectiveness(enemy.getPokemonList(CurrentEnemyPokemonInt).giveType + player1.getPokemonList(CurrentPlayerPokemonInt).giveType) > 1 Then

                switchChance = CDec(0.9) * (1 - itemChance)

                moveChance = CDec(0.1) * (1 - itemChance)

                'Else it will be more likely to choose a move in a 1:4 ratio
            Else

                switchChance = CDec(0.2) * (1 - itemChance)

                moveChance = CDec(0.8) * (1 - itemChance)

            End If

            'Decimal to store the chance of the option happening
            Dim randomChance As Decimal = CDec(Rnd())

            If randomChance > 0 And randomChance <= moveChance Then

                'If the random chance is between the move chance and 0 then the AI will choose a move
                moveAI(player1, enemy)

                'If the random chance is greater than the move chance but smaller than the move chance + the switch chance the AI will choose a pokemon to switch to
            ElseIf randomChance > moveChance And randomChance <= (moveChance + switchChance) Then

                'It will only switch to a pokemon if it has a valid pokemon to switch, at least one which isn't fainted
                If Not enemy.getPokemonList.Count - enemyFainted = 1 Then

                    'Choose a pokemon to switch one
                    switchAI(player1, enemy)

                Else

                    moveAI(player1, enemy)

                End If

                'Otherwise it will choose an Item
            Else

                'Choose and item
                itemAI(player1, enemy)

            End If

        End If

    End Sub


    'This will determine which Pokemon the AI will switch to
    Sub switchAI(ByRef player1 As Player, ByRef enemy As Player)

        'Generates new random seed
        Randomize()

        'An array of decimals store the decimal chance of each pokemon being chosen
        Dim pokemonChance(enemy.getPokemonList.Count - 1) As Decimal

        'Total points is all of the pokemon chances adde together
        Dim totalPoints As Decimal = 0

        'A list which stores the decmial chance of a pokemon being selected and the position of the pokemon in the enemy pokemon list
        Dim possiblePokemon As New List(Of AI_Switch_Choice)

        'Loops through all the pokemon in the enemy pokemon list setting how many points they will have
        For i = 0 To enemy.getPokemonList.Count - 1

            'Points depending on how effective the enemy pokemon is against player pokemon * by the HP it has, so the higher the HP and
            'The effectiveness the more likely it will get chosen
            pokemonChance(i) = effectiveness(player1.getPokemonList(CurrentPlayerPokemonInt).giveType + enemy.getPokemonList(i).giveType) * enemy.getPokemonList(i).giveHP

            'The currents pokemons points is added to the total points
            totalPoints = totalPoints + pokemonChance(i)

        Next

        'Only executes next code if total points isnt zero becuase it would crash if you try to divide by zero
        If Not totalPoints = 0 Then

            'Loops through all the pokemon in the enemy pokemon list
            For i = 0 To enemy.getPokemonList.Count - 1

                'Converts the points an option has to a decimal chance
                pokemonChance(i) = pokemonChance(i) / totalPoints

            Next

        End If

        'Sets the current enemy's pokemon chance to zero so it doesnt switch to the current pokemon
        pokemonChance(CurrentEnemyPokemonInt) = 0

        'Loops through all the pokemon in the enemy pokemon list and adds there position and decimal chance as an AI_Switch_Choice item
        'To the possible pokemon list
        For i = 0 To enemy.getPokemonList.Count - 1

            If pokemonChance(i) <= 1 Then

                'Adding the position and chance to the list
                possiblePokemon.Add(New AI_Switch_Choice(pokemonChance(i), CByte(i)))

            End If

        Next

        'Orders the list from largest chance to the smallest
        possiblePokemon = possiblePokemon.OrderBy(Function(p) p.value).Reverse.ToList

        'Stores as decimal random chance
        Dim randomChance As Decimal = CDec(Rnd())

        'Sets totalPoints to 0 as it will be used to store total decimal chances
        totalPoints = 0

        'Loops through all items in the possible pokemon list
        For i = 0 To possiblePokemon.Count - 1

            'If the random chance is between the lower bound and the pokemons decimal chance then the poekemon will be selected
            If randomChance > totalPoints And randomChance <= totalPoints + possiblePokemon(i).value Then

                'Sets the current pokemon
                CurrentEnemyPokemonInt = possiblePokemon(i).position

            End If

            'Adds decimal chance of the current pokemon to the total points
            totalPoints = totalPoints + possiblePokemon(i).value

        Next

        'If the random chance is zero or there are no viable pokemon switch choices ie. all the pokemon are not effective against
        'The current player pokemon then a random pokemon shall be chosen
        If randomChance = 0 Or totalPoints = 0 Then

            'Selects a randmon pokemon from from possible pokemon list
            CurrentEnemyPokemonInt = possiblePokemon(CInt(Math.Floor(randomChance * possiblePokemon.Count))).position

        End If

        'Adds to the log text which enemy pokemon has been sent out
        logText = logText + " Enemy sent  out " + enemy.getPokemonList(CurrentEnemyPokemonInt).giveName

    End Sub


    'This will determine which move the AI will select,AI has infinite move usage to always produce optimal move selection
    Sub moveAI(ByRef player1 As Player, ByRef enemy As Player)

        'Stores for decimals, one for each move, for the decimal chance the move will be chosen
        Dim moveChance(3) As Decimal

        Dim totalPoints As Decimal = 0

        'Stores the move the AI has chosen
        Dim selectedmove As Move

        'Stores the moves the current enemy pokemon has
        Dim availableMoves() As Move = enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves

        'Loops through all the moves, giving points to the move chance equal to the damage it would do
        For i = 0 To 3

            'Gives points to move chance i
            moveChance(i) = damageCalculation(enemy.getPokemonList(CurrentEnemyPokemonInt).giveLevel, enemy.getPokemonList(CurrentEnemyPokemonInt).giveAttack, player1.getPokemonList(CurrentPlayerPokemonInt).giveDefence, enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(i).Givetype, enemy.getPokemonList(CurrentEnemyPokemonInt).giveType, player1.getPokemonList(CurrentPlayerPokemonInt).giveType, enemy.getPokemonList(CurrentEnemyPokemonInt).giveMoves(i).GiveBaseAttack)

            'Adds move chance points to total move points
            totalPoints = totalPoints + moveChance(i)

        Next

        'If all moves are viable ie. they all do more than 0 damage, then it will execute the next code
        If Not totalPoints = 0 Then

            'Loops through all the moves
            For i = 0 To 3

                'Converts the points a move has to a decimal chance
                moveChance(i) = moveChance(i) / totalPoints

            Next

        End If

        'Stores a decimal which represents the random chance for a move
        Dim randomChance As Decimal = CDec(Rnd())

        'A block of if statments to determine where the random chance lays e.g if it is the move 1 range or the move 2 range
        If randomChance >= 0 And randomChance <= moveChance(0) Then

            selectedmove = availableMoves(0)

        ElseIf randomChance > moveChance(0) And randomChance <= (moveChance(0) + moveChance(1)) Then

            selectedmove = availableMoves(1)

        ElseIf randomChance >= (moveChance(0) + moveChance(1)) And randomChance <= (moveChance(0) + moveChance(1) + moveChance(2)) Then

            selectedmove = availableMoves(2)

        ElseIf randomChance >= (moveChance(0) + moveChance(1) + moveChance(2)) And randomChance <= (moveChance(0) + moveChance(1) + moveChance(2) + moveChance(3)) Then

            selectedmove = availableMoves(2)

            'If it doesn't lay between any of the move chances as no moves are viable ie. all moves do zero damage, then it 
            'Will pick a random move
        Else

            selectedmove = availableMoves(CInt(Math.Floor(Rnd() * 4)))

        End If

        'Generates a new random seed
        Randomize()

        'Sets randomChance to a new random value for accuracy chance
        randomChance = CDec(Rnd())

        'If the random number is less than the move accuracy then it will execute the next code, effectively giving the move the chance to succeed as the same as its accuarcy
        If randomChance > 0 And randomChance < selectedmove.GiveAccuracy Then

            'String to store the effect text
            Dim effectText As String

            'Will execute the next code if the move effect isn't no effect or the randomChance is within the move accuarcy range
            If selectedmove.giveEffectID <> 0 And randomChance < selectedmove.GiveEffectChance Then

                'Stores the effect information from the ID given by the move
                Dim moveEffect() As String = effects(selectedmove.giveEffectID).Split(CChar(","))

                'Effect text is set to the last part of the effect information
                effectText = moveEffect(5)

                'The effect will then get processed
                processEffect(player1, enemy, "E" + moveEffect(2), moveEffect(3), CDec(moveEffect(4)))

                'Else the effectText will be nothing
            Else

                effectText = ""

            End If

            'The damage of the move will be calulated
            Dim damageTaken As Integer = CInt(damageCalculation(enemy.getPokemonList(CurrentEnemyPokemonInt).giveLevel, enemy.getPokemonList(CurrentEnemyPokemonInt).giveAttack, player1.getPokemonList(CurrentPlayerPokemonInt).giveDefence, selectedmove.Givetype, enemy.getPokemonList(CurrentEnemyPokemonInt).giveType, player1.getPokemonList(CurrentPlayerPokemonInt).giveType, selectedmove.GiveBaseAttack))

            'The enemy pokemon will take the damage, decreasing its HP
            player1.getPokemonList(CurrentPlayerPokemonInt).takeDamage(damageTaken)

            'The move used and damage will be added to the log text
            logText = logText + "Enemy " + enemy.getPokemonList(CurrentEnemyPokemonInt).giveName + " used " + selectedmove.GiveName + " did " + CStr(damageTaken) + " points of damage " + effectText

            'Else the move will fail
        Else

            'The move failure will be added to the log text
            logText = logText + " Enemy move failed. "

        End If

    End Sub


    'This will determine which item the AI will use
    Sub itemAI(ByRef player1 As Player, ByRef enemy As Player)

        'Generates new random seed
        Randomize()

        'Stores the item chances as decimals
        Dim ItemChances As New List(Of Decimal)

        'Will store all points allocated to items
        Dim totalItemPoints As Decimal = 0

        'Stores the random chance for selecting an item
        Dim randomItemChance As Decimal = CDec(Rnd())

        'Stores the selected items position
        Dim selectedItemPosition As Integer = 0

        'Stores the selected item
        Dim selectedItem As Item

        'Loops through all items in the enemy's bag allocating points dependent on their stat multplier
        For i = 0 To enemy.getBag.Count - 1

            'Allocates itme points
            ItemChances.Add(enemy.getBag(i).getValue)

            'Adds current item points to total items points
            totalItemPoints += ItemChances(i)

        Next

        'If there is a viable item ie. not all items are pokeballs as enemy can't use pokeballs, then it willexecute the next code
        If totalItemPoints > 0 Then

            'Loops through all items in emeny bag converting points to percentage chance of total points
            For j = 0 To enemy.getBag.Count - 1

                'converts points to percentage chance
                ItemChances(j) = ItemChances(j) / totalItemPoints

            Next

            'Sets total item points to 0 so it can be used for storing decimal item chances
            totalItemPoints = 0

            For k = 0 To enemy.getBag.Count - 1

                'Checks if the random chance is between the lower bound and current item decimal chance range
                If randomItemChance > totalItemPoints And randomItemChance <= totalItemPoints + ItemChances(k) Then

                    'If it is then it will set selected itmes position to that items position 
                    selectedItemPosition = k

                End If

                'Adds the current items decimal chance to total points
                totalItemPoints += ItemChances(k)

            Next

        End If

        'Selected item is set to the item in the enemy's bag that is the selected item position
        selectedItem = enemy.getBag(selectedItemPosition)

        'Removes that item so that it can't be used again
        enemy.removeItem(selectedItemPosition)

        'If the selected item is not a pokeball then it will execute the next code
        If Not selectedItem.getName.Contains("ball") = True Then

            'Processess the items effect
            processEffect(player1, enemy, "ES", selectedItem.getStat, selectedItem.getValue)

            'Logs the itme used
            logText = logText + " Enemy used " + selectedItem.getName + "."

        Else

            'Logs that the AI tried to use a pokeball
            logText = logText + " Enemy tried to use pokeball. "

        End If

    End Sub


    'Processes a given effect, determines who the effect will target which stat and by how much
    Sub processEffect(ByRef player1 As Player, ByRef enemy As Player, ByVal target As String, ByVal attribute As String, ByVal percentage As Decimal)

        Select Case target

            'Enemy targets self
            Case "ES"

                applyEffect(enemy, CurrentEnemyPokemonInt, attribute, percentage)
            'Enemy targets its enemy so player
            Case "EE"

                applyEffect(player1, CurrentEnemyPokemonInt, attribute, percentage)

            'Player targets self
            Case "PS"

                applyEffect(player1, CurrentEnemyPokemonInt, attribute, percentage)

            'Player targets enemy, so enemy
            Case "PE"

                applyEffect(enemy, CurrentEnemyPokemonInt, attribute, percentage)

        End Select

    End Sub


    'Applies an effect to a targets stat by a pecentage, multiplys a stat by a percenatge
    Sub applyEffect(ByRef target As Player, ByVal pokemonInt As Integer, ByVal attribute As String, ByVal percentage As Decimal)

        Select Case attribute

            'Health
            Case "HP"

                target.getPokemonList(pokemonInt).SetHealth(CInt(target.getPokemonList(pokemonInt).giveHP * percentage))

            'Attack
            Case "ATK"

                target.getPokemonList(pokemonInt).SetAttack(CInt(target.getPokemonList(pokemonInt).giveAttack * percentage))

            'Defence
            Case "DEF"

                target.getPokemonList(pokemonInt).SetDefence(CInt(target.getPokemonList(pokemonInt).giveDefence * percentage))

            'Speed
            Case "SPD"

                target.getPokemonList(pokemonInt).SetSpeed(CInt(target.getPokemonList(pokemonInt).giveSpeed * percentage))

            'Moves accuracy
            Case "ACC"

                'Loops through all pokemon moves and modifies accuarcy
                For i = 0 To 3

                    'Modifies accuracy
                    target.getPokemonList(pokemonInt).giveMoves(i).setAccuarcy(target.getPokemonList(pokemonInt).giveMoves(i).GiveAccuracy * percentage)

                Next

        End Select

    End Sub


    'Sets the Y position of the pointer, aswell as a max limit
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


    'Damage calculator
    Function damageCalculation(ByRef attackerLevel As Integer, ByRef attackerAttack As Integer, ByRef defenderDefence As Integer, ByRef moveType As String, ByRef attackerType As String, ByRef defenderType As String, ByRef movePower As Integer) As Decimal 'returns a damage value dependent on power, attack/defence and effectiveness

        Randomize()

        Dim base, modifier, randomMultiplier, STAB, type, critcal As Decimal

        Dim rnd As New Random

        'A random multiplier
        randomMultiplier = CDec((1 - (rnd.NextDouble * (1 - 0.85))))

        'If the pokemon is the same type as the move then there is an attack bonus
        If moveType = attackerType Then

            STAB = CDec(1.5)

        Else

            STAB = CDec(1)

        End If

        'Effectivesness attack bonus
        type = effectiveness(defenderType + moveType)

        Randomize()

        '1/20 chance that the attack is a critical attack
        If rnd.NextDouble < 0.05 Then

            critcal = CDec(1.5)

        Else

            critcal = CDec(1)

        End If

        'Default damage value
        base = CDec((((((2 * attackerLevel) / 5) + 2) * movePower * (attackerAttack / defenderDefence)) / 50) + 2)

        modifier = randomMultiplier * type * critcal * STAB

        Return base * modifier

    End Function



End Class