'Map class stores the Interactle parts of the map,NPCs as well as where the player can traverse
Public Class Map

    'The map height in number of cells
    Private numberOfRows As Integer

    'The map width in number of cells
    Private numberOfColumns As Integer

    'The maze which the player walks in
    Private maze(,) As Cell

    'The NPCs the player can interact with
    Private entityMap(,) As Player

    'Used to store cells in the depth withs search with recursive backtracking alogrithim
    Private stackOfCells As Stack(Of Cell)

    'Stores the file loaction for the tiles used when drawing the game
    Private tileFileLocation As String

    'Stores how much time has passed in the map
    Private timer As New TimeSpan


    'Creates a new map, this is the contstructor class, which will create a new map when one is intantiated.
    Sub New(ByVal x As Byte, ByVal y As Byte, ByVal exisiting As Boolean, ByVal saveLocation As String)

        'Sets the height of the map
        numberOfRows = x

        'Sets the width of the map
        numberOfColumns = y

        'Resizes the maze array
        ReDim MyClass.maze(numberOfRows, numberOfColumns)

        'Resizes the entity map array
        ReDim MyClass.entityMap(numberOfRows, numberOfColumns)

        'Starts the game timer at zero seconds
        timer = TimeSpan.Zero

        'If the game isn't being loaded it will exceute the following code
        If exisiting = False Then

            'Instantiates the stack of cells
            stackOfCells = New Stack(Of Cell)

            'Default tile file location
            tileFileLocation = "tile2"

            'Loops through the array of the maze, populating it with new cells
            For y = 0 To CByte((numberOfRows - 1))

                For x = 0 To CByte(numberOfColumns - 1)

                    'Sets the specified array position to a new cell
                    maze(x, y) = New Cell(x, y)

                Next

            Next

            'places empty rooms in the map
            generateRoom()

            'sets the starting point of the DFS recursive backtracking alogorithim
            maze(0, 0).setVisited(True)

            'starts the DFS recursive backtracking alogorithim
            Algorithim(maze(0, 0))

            'sets the symbol pf the cells in the maze
            For y = 0 To CByte(numberOfRows - 1)

                For x = 0 To CByte(numberOfColumns - 1)

                    maze(x, y).setSymbol(maze(x, y).getNorth, maze(x, y).getEast, maze(x, y).getSouth, maze(x, y).getWest)

                Next

            Next

            'Else the save file will be loaded and
        Else

            'Loads the save file
            Dim saveFile As New System.IO.StreamReader(saveLocation)

            'Saves the lines of the save file in a lsit
            Dim saveFileAsList As New List(Of String)

            'Loops till the end of the file
            While Not saveFile.EndOfStream

                'Adds the current line of the file to the list
                saveFileAsList.Add(Convert.ToString(saveFile.ReadLine))

            End While

            'Stores the current line of the file being processed
            Dim currentLineInSaveFileString() As String

            'Splits the first line of the file, the map settings line
            currentLineInSaveFileString = saveFileAsList(0).Split(CChar(";"))

            'Setsd the tile file location
            tileFileLocation = currentLineInSaveFileString(2)

            'Loops through the array of the maze, populating it with new cells
            For y = 0 To CByte((numberOfRows - 1))

                For x = 0 To CByte(numberOfColumns - 1)

                    'Sets the specified array position to a new cell
                    maze(x, y) = New Cell(x, y)

                Next

            Next

            'sets the symbol pf the cells in the maze
            For Ty = 0 To numberOfRows - 1

                'Splits the current line in the file into the individula cells
                currentLineInSaveFileString = saveFileAsList(Ty + 1).Split(CChar(";"))

                For Tx = 0 To numberOfColumns - 1

                    'Sets the current cells sysmbol
                    maze(Tx, Ty).setCellFromSymbol(CInt(currentLineInSaveFileString(Tx)))

                Next

            Next

            'Stores the current entity being processed
            Dim currentEntity() As String

            'Loops through entity map
            For Ey = 0 To numberOfRows - 1

                'Splits current line in file, into individual entity
                currentLineInSaveFileString = saveFileAsList(Ey + numberOfRows + 2).Split(CChar(";"))

                For Ex = 0 To numberOfColumns - 1

                    'There is an entity at the point in the save data it will execute the next code
                    If currentLineInSaveFileString(Ex).Length > 0 Then

                        'Splits the enetity
                        currentEntity = currentLineInSaveFileString(Ex).Split(CChar(":"))

                        'Gets the entity's name
                        Select Case currentEntity(0)

                            Case "NPC"

                                'Creates a new NPC at the specified position
                                entityMap(Ex, Ey) = New NPC(Ex, Ey, Game1.pokedex, Game1.itemdex, True)

                                'Gets how many items are in the MPCs bag
                                Dim NPCbagCount As Integer = CInt(currentEntity(1))

                                'Loops through the number of items in the players bag
                                For i = 1 To NPCbagCount

                                    'Adds the items into the NPCs bag
                                    entityMap(Ex, Ey).getitem(Game1.itemdex(CInt(currentEntity(1 + i))))

                                Next

                                'Stores how many pokemon the NPc has
                                Dim NPCpokemonCount As Integer = CInt(currentEntity(NPCbagCount + 2))

                                'Stores the pokemons save data
                                Dim currentPokemonString() As String

                                'Stores the pokemons moves
                                Dim currentMoveString() As String

                                'Loops for the number of pokemon
                                For p = NPCbagCount + 3 To NPCpokemonCount + NPCbagCount + 2

                                    'Splits the current pokemons save data
                                    currentPokemonString = currentEntity(p).Split(CChar(","))

                                    'Gives the NPC a pokemon
                                    entityMap(Ex, Ey).getpokemon(New Pokemon(Game1.pokedex(CInt(currentPokemonString(0))).giveName, Game1.pokedex(CInt(currentPokemonString(0))).giveLevel, Game1.pokedex(CInt(currentPokemonString(0))).giveMaxHP, Game1.pokedex(CInt(currentPokemonString(0))).giveMoves(0), Game1.pokedex(CInt(currentPokemonString(0))).giveMoves(1), Game1.pokedex(CInt(currentPokemonString(0))).giveMoves(2), Game1.pokedex(CInt(currentPokemonString(0))).giveMoves(3), Game1.pokedex(CInt(currentPokemonString(0))).giveType, Game1.pokedex(CInt(currentPokemonString(0))).giveAttack, Game1.pokedex(CInt(currentPokemonString(0))).giveDefence, Game1.pokedex(CInt(currentPokemonString(0))).giveSpeed, Game1.pokedex(CInt(currentPokemonString(0))).giveTextureAsString, Game1.pokedex(CInt(currentPokemonString(0))).giveID))

                                    'Sets the pokemons level
                                    entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).SetLevel(CInt(currentPokemonString(1)))

                                    'Sets the pokemons health
                                    entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).SetHealth(CInt(currentPokemonString(2)))

                                    'Loops for times - the number of moves a pokemon has
                                    For i = 0 To 3

                                        'Splits the current move, bar deliminated
                                        currentMoveString = currentPokemonString(3 + i).Split(CChar("|"))

                                        'Gives the pokemon the move
                                        entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).setMove(i, New Move(Game1.movedex(CInt(currentMoveString(0))).getMoveAsString))

                                        'Sets the current uses of the moves
                                        entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).giveMoves(i).setPP(CInt(currentMoveString(1)))

                                    Next

                                    'Sets the attack of the pokemon
                                    entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).SetAttack(CInt(currentPokemonString(7)))

                                    'Sets the defence of the pokemon
                                    entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).SetDefence(CInt(currentPokemonString((8))))

                                    'Sets the speed of the pokemon
                                    entityMap(Ex, Ey).getPokemonList(p - (NPCbagCount + 3)).SetSpeed(CInt(currentPokemonString((9))))

                                Next

                            Case "SIGN"

                                'Stores the signs save data 
                                Dim signAsString() As String = currentLineInSaveFileString(Ex).Split(CChar(":"))

                                'Gets the signs text and creates a new sign
                                entityMap(Ex, Ey) = New Sign(Ex, Ey, CStr(signAsString(1)))

                            Case "CHEST"

                                'Saves the chests save data
                                Dim chestAsString() As String = currentLineInSaveFileString(Ex).Split(CChar(":"))

                                'Stores the number of items in the chest
                                Dim ChestbagCount As Integer = CInt(chestAsString(1))

                                'Creates a new chest
                                entityMap(Ex, Ey) = New Chest(Ex, Ey, Game1.itemdex, True)

                                'Loops for the number of items in the chest
                                For i = 2 To ChestbagCount + 1

                                    'Adds the currrent item to the chest
                                    entityMap(Ex, Ey).getitem(Game1.itemdex(CInt(chestAsString(i))))

                                Next

                        End Select

                    End If

                Next

            Next

            'Stores the players save data
            Dim playerAsString() As String = (saveFileAsList(saveFileAsList.Count - 1)).Split(CChar(";"))

            'Creates a new player
            Game1.player = New Player()

            'Sets the players name
            Game1.player.setName(playerAsString(0))

            'Sets the players X position in the maze
            Game1.player.setx(CInt(playerAsString(1)))

            'Sets the players Y position in the maze
            Game1.player.sety(CInt(playerAsString(2)))

            'Sets the users X position on the screen
            Game1.player.setpx(CInt(playerAsString(3)))

            'Sets the users Y position on the creen
            Game1.player.setpy(CInt(playerAsString(4)))

            'Stores the number of itme sin the players bag
            Dim PlayerbagCount As Integer = CInt(playerAsString(5))

            'Loops for the number of items in the players bag
            For i = 5 To PlayerbagCount + 5

                'Adds the current item to the players bag
                Game1.player.getitem(Game1.itemdex(CInt(playerAsString(1 + i))))

            Next

            'Stores the number of pokemon the player has
            Dim pokemonCount As Integer = CInt(playerAsString(PlayerbagCount + 6))

            'Stores the current pokemons save data
            Dim currentPlayerPokemonString() As String

            'stores the current pokemons current move data
            Dim currentPlayerMoveString() As String

            'Loops for the number of pokemon the player has
            For p = PlayerbagCount + 7 To pokemonCount + PlayerbagCount + 6

                'Sets the current pokemon save data
                currentPlayerPokemonString = playerAsString(p).Split(CChar(","))

                'Gives the player the pokemon
                Game1.player.getpokemon(New Pokemon(Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveName, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveLevel, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveMaxHP, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveMoves(0), Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveMoves(1), Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveMoves(2), Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveMoves(3), Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveType, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveAttack, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveDefence, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveSpeed, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveTextureAsString, Game1.pokedex(CInt(currentPlayerPokemonString(0))).giveID))

                'Sets the pokemons level
                Game1.player.getPokemonList(p - (PlayerbagCount + 7)).SetLevel(CInt(currentPlayerPokemonString(1)))

                'Sets the pokemons Health
                Game1.player.getPokemonList(p - (PlayerbagCount + 7)).SetHealth(CInt(currentPlayerPokemonString((2))))

                'Loops for times - the number of moves a pokemon has
                For i = 0 To 3

                    'Splits the move save data
                    currentPlayerMoveString = currentPlayerPokemonString(3 + i).Split(CChar("|"))

                    'Gives the pokemon the current move
                    Game1.player.getPokemonList(p - (PlayerbagCount + 7)).setMove(i, New Move(Game1.movedex(CInt(currentPlayerMoveString(0))).getMoveAsString))

                    'Gives sets the current moves uses
                    Game1.player.getPokemonList(p - (PlayerbagCount + 7)).giveMoves(i).setPP(CInt(currentPlayerMoveString(1)))

                Next

                'Sets the current pokemons Attack stat
                Game1.player.getPokemonList(p - (PlayerbagCount + 7)).SetAttack(CInt(currentPlayerPokemonString((7))))

                'Sets the currrent pokemons Defence stat
                Game1.player.getPokemonList(p - (PlayerbagCount + 7)).SetDefence(CInt(currentPlayerPokemonString((8))))

                'Sets the current pokeomns speed stat
                Game1.player.getPokemonList(p - (PlayerbagCount + 7)).SetSpeed(CInt(currentPlayerPokemonString((9))))

            Next

            'Closes the save file as it is no longer required
            saveFile.Close()

        End If

    End Sub


    'Generates the maze
    Sub Algorithim(ByRef current As Cell)

        'Gets a random neighbourgh cell
        Dim neighbourghCell As Cell = CheckNeighbourghs(maze(current.getX, current.getY))

        'If the current cell has no neighbouroghs
        If neighbourghCell.getX = 0 And neighbourghCell.getY = 0 Then


            If stackOfCells.Count > 0 Then

                'Sends the previous previous cell to the algorithim - backtracking
                Algorithim(stackOfCells.Pop())

            End If

        Else

            'Marks the neighbourgh cell as visited
            maze(neighbourghCell.getX, neighbourghCell.getY).setVisited(True)

            'Puts the nieghbourgh onto the stack
            stackOfCells.Push(current)

            'Removes the walls between the current cell and the neighbourgh cell
            RemoveWalls(maze(current.getX, current.getY), maze(neighbourghCell.getX, neighbourghCell.getY))

            'Calls the alogrithim again
            Algorithim(neighbourghCell)

        End If

    End Sub


    'Returns a random neighbourgh next to a given cell
    Function CheckNeighbourghs(ByRef current As Cell) As Cell

        'Stores the neighbourgh cells
        Dim neighbours As New List(Of Cell)

        'If the right cell of the current cell isn't out of the maze, execute the next code
        If current.getX + 1 < numberOfColumns Then

            'If the right cell hasn't been visited
            If maze(current.getX + 1, current.getY).getVisited = False Then

                'Add the right cell to the list of available neighbourghs
                neighbours.Add(maze(current.getX + 1, current.getY))

            End If

        End If

        'If the cell to the left isn't out of the maze
        If current.getX - 1 > -1 Then

            'If the right cell isn't visited
            If maze(current.getX - 1, current.getY).getVisited = False Then

                'Add the left cell to the list of available neighbourghs
                neighbours.Add(maze(current.getX - 1, current.getY))

            End If

        End If

        'If the below cells isn't out of the maze
        If current.getY + 1 < numberOfRows Then

            'If the below cell isn't visited
            If maze(current.getX, current.getY + 1).getVisited = False Then

                'Add the below cell to the lsit of available neighbourghs
                neighbours.Add(maze(current.getX, current.getY + 1))

            End If

        End If

        'If the above cell isn't out of the maze
        If current.getY - 1 > -1 Then

            'Ig the above cell isn't visited
            If maze(current.getX, current.getY - 1).getVisited = False Then

                'add the above cell to the list of available cells
                neighbours.Add(maze(current.getX, current.getY - 1))

            End If

        End If

        'Generates a new random seed
        Randomize()

        'If there is an avaiable nighbourgh
        If neighbours.Count > 0 Then

            'Retrun a random neighbourgh cell
            Return neighbours(CInt(Math.Floor(Rnd() * neighbours.Count)))

        Else

            'Retrun a "no available cell"
            Return New Cell(0, 0)

        End If

    End Function


    'Removes the walls between two given cells
    Sub RemoveWalls(ByRef current As Cell, ByRef neighbour As Cell)

        'Calculates the differce in X position
        Dim xResult As Integer = current.getX - neighbour.getX

        'Calculates the difference in Y position
        Dim yResult As Integer = current.getY - neighbour.getY

        'If the cells are on the same row
        If xResult = 0 Then

            'If the current cell is above the neighbourgh cell
            If yResult = -1 Then

                'Removes the current cells bottom wall
                current.setSouth(False)

                'Removes the neighbourghs top wall
                neighbour.setNorth(False)

            Else

                'Removes the current cells top wall
                current.setNorth(False)

                'Removes the neighbourghs bottom wall
                neighbour.setSouth(False)

            End If

            'If the cells are on different rows
        ElseIf xResult = -1 Then

            'Removes the current cells right wall
            current.setEast(False)

            'Removes the neighbourghs left wall
            neighbour.setWest(False)

        Else

            'Removes the current cells left wall
            current.setWest(False)

            'Removes the neighbourghs right wall
            neighbour.setEast(False)

        End If

    End Sub


    'Creates empty spaces within the maze for tall grass
    Sub generateRoom()

        Dim usedCoordinates As New List(Of String(,))

        'Number of rooms the subroutine will try to generate
        Const numberOfRooms = 9

        'The room size, as a square, in number of cells
        Const roomSize = 5

        'Stores the current rooms coordinate
        Dim tempArray(roomSize - 1, roomSize - 1) As String

        'Random X position
        Dim rndX As Integer

        'Random Y position
        Dim rndY As Integer

        'Boolean if the coordinate as already been used
        Dim inUsedCoordinates As Boolean

        'Loops for the number of rooms
        For n = 0 To numberOfRooms - 1

            Do

                'Generates a random seed
                Randomize()

                'Generates a new random x position, 5 - 20
                rndX = CInt(Math.Floor(Rnd() * 21) + 5)

                'Generates a new random y position, 5 - 20
                rndY = CInt(Math.Floor(Rnd() * 21) + 5)

                'Sets that it is not in used coordinates
                inUsedCoordinates = False

                'Loops through the list which store the array of room coordinates
                For c = 0 To usedCoordinates.Count - 1

                    'Loops through current array
                    For y = 0 To roomSize - 1

                        For x = 0 To roomSize - 1

                            'Checks if the ranom coordinate is already in use
                            If usedCoordinates(c)(x, y) = CStr(rndX) + CStr(rndY) Then

                                'If so sets it to be in the used coordinates to be true
                                inUsedCoordinates = True

                            End If

                        Next

                    Next

                Next

                'Loops until it finds unocupied coordinates
            Loop Until inUsedCoordinates = False

            'Loops through the rooms coordinates
            For y = rndY To rndY + (roomSize - 1)

                For x = rndX To rndX + (roomSize - 1)

                    'Adds them to an array
                    tempArray(x - rndX, y - rndY) = CStr(x) + CStr(y)

                Next

            Next

            'Adds the array of coordinates to the list of used coordinates
            usedCoordinates.Add(tempArray)

            'Loops through the current coordinates in the room
            For y = rndY To rndY + roomSize - 1

                For x = rndX To rndX + roomSize - 1

                    'Sets all walls to be false and sets its symbol as 0
                    maze(x, y).empty()

                Next

            Next

            'Defaults inusedcoordinates boolean
            inUsedCoordinates = True

        Next

    End Sub


    'Adds Entities to the map - NPCs, SIGNS and CHESTs
    Sub populateMapWithNPC(ByRef pokedex As List(Of Pokemon), ByRef itemdex As List(Of Item))

        'Stores random X and Y coordinates
        Dim rndX, rndY As Integer

        'Number of NPC spawn attempts
        Const numberOfNPC As Integer = 50

        'Number of chest spawn attempts
        Const numberOfChests As Integer = 50

        'Number of sign spawn attempts
        Const numberOfSigns As Integer = 50

        'List of used coordinates
        Dim usedCoordinates As New List(Of String)

        'Stores if current random coordinates is in use or not
        Dim inUsedCoordinates As Boolean = True

        'Stores the text the signs can have
        Dim ListOfSignText As New List(Of String)

        'Tries to execute next code
        Try

            'Loads sign text file
            Dim signTextFile As New IO.StreamReader("content/sign text.txt")

            'Loops till end of file
            While Not signTextFile.EndOfStream

                'Adds current line of file to list
                ListOfSignText.Add(signTextFile.ReadLine)

            End While

            'Closes file as it is no longer required
            signTextFile.Close()

            'Loops for number of spawn tries of signs
            For SIGN = 1 To numberOfSigns

                'Loops until it finds new unoccupied coordinates
                Do

                    'Defaults inCoordinates to false
                    inUsedCoordinates = False

                    'Generates new random seed
                    Randomize()

                    'Creates new random X position - anywhere in the map
                    rndX = CInt(Math.Floor(Rnd() * numberOfRows))

                    'Creates new random Y position - anywhere in the map
                    rndY = CInt(Math.Floor(Rnd() * numberOfRows))

                    'Loops through all the coordinates in the used coordinate list
                    For i = 0 To usedCoordinates.Count - 1

                        'Checks if the new coordinate is already in use
                        If usedCoordinates(i) = CStr(rndX) + CStr(rndY) Then

                            'If true sets boolean to true
                            inUsedCoordinates = True

                        End If

                    Next

                Loop Until inUsedCoordinates = False

                'Generates a new random seed
                Randomize()

                'Adds the new coordinate to the used coordinate list
                usedCoordinates.Add((CStr(rndX) + CStr(rndY)))

                'Creates a new signs, assigns new random text
                entityMap(rndX, rndY) = New Sign(rndX, rndY, ListOfSignText(CInt(Math.Floor(Rnd() * (ListOfSignText.Count - 1)))))

            Next

            'Catches any errors
        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading ""sign text.txt"" file. " + ex.Message + " No signs will be placed.")

        End Try

        'Loops for the number of NPC spawn tries
        For NPC = 1 To numberOfNPC

            'Loops until it finds new unoccupied coordinates
            Do

                'Defaults boolean to false
                inUsedCoordinates = False

                'Generates new random seed
                Randomize()

                'Creates new random X position - anywhere in the map
                rndX = CInt(Math.Floor(Rnd() * numberOfRows))

                'Creates new random Y position - anywhere in the map
                rndY = CInt(Math.Floor(Rnd() * numberOfRows))

                'Loops through all the coordinates in the used coordinate list
                For i = 0 To usedCoordinates.Count - 1

                    'Checks if the new coordinate is already in use
                    If usedCoordinates(i) = CStr(rndX) + CStr(rndY) Then

                        'If true sets boolean to true
                        inUsedCoordinates = True

                    End If

                Next

            Loop Until inUsedCoordinates = False

            'Adds the new coordinate to the used coordinate list
            usedCoordinates.Add((CStr(rndX) + CStr(rndY)))

            'Creates new NPC at the random coordinates
            entityMap(rndX, rndY) = New NPC(rndX, rndY, pokedex, itemdex, False)

        Next

        'Loops for the number of chest spawn tries
        For CHEST = 1 To numberOfChests

            'Loops until it finds new unoccupied coordinates
            Do

                'Defaults boolean to false
                inUsedCoordinates = False

                'Generates new random seed
                Randomize()

                'Creates new random X position - anywhere in the map
                rndX = CInt(Math.Floor(Rnd() * numberOfRows))

                'Creates new random Y position - anywhere in the map
                rndY = CInt(Math.Floor(Rnd() * numberOfRows))

                'Loops through all the coordinates in the used coordinate list
                For i = 0 To usedCoordinates.Count - 1

                    'Checks if the new coordinate is already in use
                    If usedCoordinates(i) = CStr(rndX) + CStr(rndY) Then

                        'If true sets boolean to true
                        inUsedCoordinates = True

                    End If

                Next

            Loop Until inUsedCoordinates = False

            'Adds the new coordinate to the used coordinate list
            usedCoordinates.Add((CStr(rndX) + CStr(rndY)))

            'Creates a new chest at the random coordinates
            entityMap(rndX, rndY) = New Chest(rndX, rndY, itemdex, False)

        Next

    End Sub

    'Returns a specific tile from within the maze
    Function tile(ByVal x As Integer, ByVal y As Integer) As Cell

        Return maze(x, y)

    End Function


    'Returns the height of the map in cells
    Function getHeight() As Integer

        Return numberOfRows

    End Function


    'Returns the width of the map in cells
    Function getWidth() As Integer

        Return numberOfColumns

    End Function


    'Returns at entity as specific coordinates of the entity map
    Function getEntity(ByVal xPos As Integer, ByVal yPos As Integer) As Player

        If entityMap(xPos, yPos) IsNot Nothing Then

            Return entityMap(xPos, yPos)

        Else

            Return Nothing

        End If

    End Function


    'Updates the maps logic
    Sub Update(ByVal playerXpos As Integer, ByVal playerYpos As Integer, ByVal gameTime As GameTime)

        'Gets which key has been pressed
        Game1.kbState = Keyboard.GetState

        'Interacts with enetity
        If Game1.kbState.IsKeyDown(Keys.T) And Game1.gameState = "Playing" Then

            'Makes sure there is an entity at the players location to interact with to preevtncrashes
            If entityMap(playerXpos, playerYpos) IsNot Nothing Then

                'Plays sound effect
                Game1.pressA.Play()

                Threading.Thread.Sleep(70)

                'Activates the entities interaction
                entityMap(playerXpos, playerYpos).interact()

            End If

            'Opens entity
        ElseIf Game1.kbState.IsKeyDown(Keys.E) And Game1.gameState = "Playing" Then

            Game1.gameState = "Menu"

            'Plays sound effect
            Game1.pressA.Play()

            Threading.Thread.Sleep(70)

            'Creates the menu GUI
            Game1.messageBox = New MessageBox(loadMenuOptions, True, True)

        End If

        'Creates a new random
        Dim randomizer As New Random()

        'Increases the map timer
        timer += gameTime.ElapsedGameTime

        'Resets the map timer if its greater than 4 seconds
        If timer > TimeSpan.FromSeconds(4) Then

            timer = TimeSpan.Zero

        End If

        'If the player is in tall grass
        If maze(playerXpos, playerYpos).getSymbol = 0 Then

            'Generate a random seed
            Randomize()

            'If the mpa timer equals the random number then an encounter begins
            If timer = TimeSpan.FromSeconds(randomizer.Next(3)) And Game1.gameState = "Playing" Then

                'Change game state to encounter
                Game1.gameState = "Encounter"

                'Creates a new encounter player to hold pokemon
                Game1.encounterPokemon = New Player

                'Creates a random number upto the pokedex count
                Dim randomPokemonPosition As Integer = CInt(Math.Floor(Rnd() * Game1.pokedex.Count))

                'Gives the encounter player a random pokemon
                Game1.encounterPokemon.getpokemon(New Pokemon(Game1.pokedex(randomPokemonPosition).giveName, Game1.pokedex(randomPokemonPosition).giveLevel, Game1.pokedex(randomPokemonPosition).giveMaxHP, Game1.pokedex(randomPokemonPosition).giveMoves(0), Game1.pokedex(randomPokemonPosition).giveMoves(1), Game1.pokedex(randomPokemonPosition).giveMoves(2), Game1.pokedex(randomPokemonPosition).giveMoves(3), Game1.pokedex(randomPokemonPosition).giveType, Game1.pokedex(randomPokemonPosition).giveAttack, Game1.pokedex(randomPokemonPosition).giveDefence, Game1.pokedex(randomPokemonPosition).giveSpeed, Game1.pokedex(randomPokemonPosition).giveTextureAsString, Game1.pokedex(randomPokemonPosition).giveID))

                'Creates a new battle
                Game1.gameBattle = New Battle(Game1.player, Game1.encounterPokemon, True)

            End If

        End If

    End Sub


    'Loads the in game menu options and returns them as a list of strings
    Function loadMenuOptions() As List(Of String)

        'List to store the menu options as string
        Dim MenuOptions As New List(Of String)

        'Tries to execute next code
        Try

            'Loads file
            Dim MenuOptionsFile As New IO.StreamReader("content/menu options.txt")

            'Loops till end of file
            While Not MenuOptionsFile.EndOfStream

                'Adds current file line to list
                MenuOptions.Add(MenuOptionsFile.ReadLine)

            End While

            'Closes file as it is no longer required
            MenuOptionsFile.Close()

            'Catches any errors
        Catch ex As Exception

            'Displays error message
            MsgBox("Error loading menu options ""menu options.txt"" file. " + ex.Message)

            'Adds default options
            MenuOptions.Add("Pokemon")

            MenuOptions.Add("Bag")

            MenuOptions.Add("Save")

            MenuOptions.Add("Exit")

        End Try

        'Returns list of menu options
        Return MenuOptions

    End Function


    'Draws the map
    Sub drawAll(ByRef tiles As SpriteBatch, ByVal mapHeight As Integer, ByVal mapWidth As Integer, ByVal playerXpos As Integer, ByVal playerYpos As Integer)

        'X,Y coordinate for the tile textures
        Dim a, b As Integer

        Dim sourceRectange, destinationRectangle As Rectangle

        'Stores the tiles size in pixels - tile is square
        Dim _tileSize As Integer = 16

        'Number of tile textures in a row
        Dim _tileMapTextureColumns As Integer = 8

        Try

            'Tile texture
            Dim _tileMapTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)(tileFileLocation)

            'Sign texture
            Dim _signTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("sign3")

            'NPC texture
            Dim _NPCTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("NPC2")

            'Chest texture
            Dim _ChestTexture As Texture2D = Game1.ContentLoader.Load(Of Texture2D)("chest2")

            Dim currentEntityName As String = ""

            'Chest texture size in pixels - square
            Const chestTextureSize As Integer = 14

            tiles.Begin()

            'Loops through maze
            For y = 0 To mapHeight - 1

                For x = 0 To mapWidth - 1

                    'Texture X position
                    a = (maze(x, y).getSymbol Mod _tileMapTextureColumns) * _tileSize

                    'Texture Y position
                    b = CInt(Math.Floor(maze(x, y).getSymbol / _tileMapTextureColumns)) * _tileSize

                    sourceRectange = New Rectangle(a, b, _tileSize, _tileSize)

                    destinationRectangle = New Rectangle((x * _tileSize), (y * _tileSize), _tileSize, _tileSize)

                    'Draws tile
                    tiles.Draw(_tileMapTexture, destinationRectangle, sourceRectange, Color.White)

                    'If there is an entity at the current coordinate
                    If entityMap(x, y) IsNot Nothing Then

                        'Assigns the current entity's name
                        currentEntityName = entityMap(x, y).getName

                    End If

                    'Selects entity's name
                    Select Case currentEntityName

                        Case "SIGN"

                            sourceRectange = New Rectangle(0, 0, _tileSize, _tileSize)

                            destinationRectangle = New Rectangle((x * _tileSize), (y * _tileSize), _tileSize, _tileSize)

                            'Draws sign
                            tiles.Draw(_signTexture, destinationRectangle, sourceRectange, Color.White)

                        Case "CHEST"

                            sourceRectange = New Rectangle(0, 0, chestTextureSize, chestTextureSize)

                            destinationRectangle = New Rectangle((x * _tileSize) + (_tileSize - chestTextureSize), (y * _tileSize) + (_tileSize - chestTextureSize), chestTextureSize, chestTextureSize)

                            'Draws chest
                            tiles.Draw(_ChestTexture, destinationRectangle, sourceRectange, Color.White)

                        Case "NPC"

                            sourceRectange = New Rectangle(0, 0, _tileSize, _tileSize)

                            destinationRectangle = New Rectangle((x * _tileSize), (y * _tileSize), _tileSize, _tileSize)

                            'Draws NPC
                            tiles.Draw(_NPCTexture, destinationRectangle, sourceRectange, Color.White)

                    End Select

                    'Resets current entity's name
                    currentEntityName = ""

                Next

            Next

            tiles.End()

            'Ctaches any crashes
        Catch ex As Exception

            'Displays error message
            MsgBox("Error Drawing map. " + ex.Message)

            'Returns user to main menu
            Game1.gameState = "MainMenu"

        End Try

    End Sub


    'Returns the map save data
    Function getSaveConstAsString() As String

        'Returns the width, height and save loaction for the tiles
        Return CStr(numberOfRows) + ";" + CStr(numberOfColumns) + ";" + tileFileLocation

    End Function


    'Returns the maze save data
    Function getSaveMazeList() As List(Of String)

        'Stores the sysmbols of the cells in the map
        Dim listOfCells As New List(Of String)

        For y = 0 To numberOfColumns - 1

            For x = 0 To numberOfRows - 1

                If Not x = numberOfRows Then

                    'Adds sysmbol to list
                    listOfCells.Add(CStr(maze(x, y).getSymbol) + ";")

                Else

                    'Last sysmnbol in the row doesn't require a semi colon
                    listOfCells.Add(CStr(maze(x, y).getSymbol))

                End If

            Next

        Next

        'Returns list of symbols
        Return listOfCells

    End Function


    'Returns the entity map save data
    Function getEntityMapList() As List(Of String)

        'List of entity data
        Dim listOfCells As New List(Of String)

        'Loops through the entity map
        For y = 0 To numberOfColumns - 1

            For x = 0 To numberOfRows - 1

                'Save entity data
                If Not x = numberOfRows And IsNothing(entityMap(x, y)) = False Then

                    listOfCells.Add(CStr(entityMap(x, y).getSaveString) + ";")

                ElseIf IsNothing(entityMap(x, y)) = False Then

                    listOfCells.Add(CStr(entityMap(x, y).getSaveString))

                    'If the entity is nothing puts a blank space
                ElseIf IsNothing(entityMap(x, y)) Then

                    listOfCells.Add(";")

                ElseIf IsNothing(entityMap(x, y)) And x = numberOfRows Then

                    listOfCells.Add("")

                End If

            Next

        Next

        'Returns entity save data list
        Return listOfCells

    End Function


End Class