'Move Class
Public Class Move

    'Move ID - uniquw
    Private ID As Integer

    'Move name
    Private name As String

    'Move type
    Private type As String

    'Move base power
    Private power As Integer

    'Move accuarcy as decimal
    Private accuracy As Decimal

    'Number of times the move can used
    Private PP As Integer

    'Move priority, 0 or 1, 1 being most prioritised
    Private priority As Byte

    'Damage type, physical(Ph)/Special(SP/Status(St)
    Private damageType As String

    'Effect ID - unique
    Private effectID As Integer

    'Effect chance as decimal
    Private effectChance As Decimal


    'Creates new move
    Sub New(ByVal c() As String)

        'Sets attributes
        ID = CInt(c(0))

        name = c(1)

        type = c(2)

        power = CInt(c(3))

        accuracy = CDec(c(4))

        PP = CInt(c(5))

        priority = CByte(c(6))

        damageType = c(7)

        effectID = CInt(c(8))

        effectChance = CDec(c(9))

    End Sub


    'Returns base power
    Function GiveBaseAttack() As Integer

        Return power

    End Function


    'Returns its self as an array of strings
    Function getMoveAsString() As String()

        Dim moveAsArray(9) As String

        moveAsArray(0) = CStr(ID)

        moveAsArray(1) = name

        moveAsArray(2) = type

        moveAsArray(3) = CStr(power)

        moveAsArray(4) = CStr(accuracy)

        moveAsArray(5) = CStr(PP)

        moveAsArray(6) = CStr(priority)

        moveAsArray(7) = CStr(damageType)

        moveAsArray(8) = CStr(effectID)

        moveAsArray(9) = CStr(effectChance)

        Return moveAsArray

    End Function


    'REturns the moves name
    Function GiveName() As String

        Return name

    End Function


    'Returns the moves type
    Function Givetype() As String

        Return type

    End Function


    'Returns the moves effect chance
    Function GiveEffectChance() As Decimal

        Return effectChance

    End Function


    'Returns the effects ID
    Function giveEffectID() As Integer

        Return effectID

    End Function


    'Returns the moves accuarcy
    Function GiveAccuracy() As Decimal

        Return accuracy

    End Function


    'Returns the moves uses left
    Function givePP() As Integer

        Return PP

    End Function


    'Sets the moves base attck from a given value
    Sub setBaseAttack(ByVal p As Integer)

        power = p

    End Sub



    'Sets the moves accuarcy from a given value
    Sub setAccuarcy(ByVal a As Decimal)

        accuracy = a

    End Sub


    'Sets the moves uses left
    Sub setPP(ByRef newPP As Integer)

        PP = newPP

    End Sub



    'Decreases moves uses left by 1
    Sub decreasePP()

        PP -= 1

        'If moves uses left is tried to be set less than zero it default pp to the lower bound - 0
        If PP < 0 Then

            PP = 0

        End If

    End Sub


    'Returns the moves save data as a string
    Function getSaveString() As String

        'Saves the moves ID and uses left, bar delimnated
        Return (CStr(ID) + "|" + CStr(PP))

    End Function


End Class
