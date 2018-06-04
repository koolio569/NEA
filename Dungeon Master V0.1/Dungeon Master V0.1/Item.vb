'An item class
Public Class Item

    'Item ID unique
    Private ID As Integer

    'Item name
    Private name As String

    'Stat item effect
    Private stat As String

    'decimal percenatge of how much it effects the stat
    Private value As Decimal

    'Item texture location
    Private spriteLocation As String

    'Item texture width
    Private spriteWidth As Integer

    'Item texture height
    Private spriteHeight As Integer


    'Creates new item
    Sub New(ByVal c() As String)

        ID = CInt(c(0))

        name = c(1)

        stat = c(2)

        value = CDec(c(3))

        Dim currentSpriteInfo() As String

        currentSpriteInfo = c(4).Split(CChar(":"))

        spriteLocation = currentSpriteInfo(0)

        spriteWidth = CInt(currentSpriteInfo(1))

        spriteHeight = CInt(currentSpriteInfo(2))

    End Sub

    'Returns sprite location
    Function getSpriteLocation() As String

        Return spriteLocation

    End Function


    'Retruns sprites width
    Function getSpriteWidth() As Integer

        Return spriteWidth

    End Function


    'Returns sprites height
    Function getSpriteHeight() As Integer

        Return spriteHeight

    End Function


    'Returns item name
    Function getName() As String

        Return name

    End Function


    'Returns items stat value modifier
    Function getValue() As Decimal

        Return value

    End Function


    'Returns stat which item effects
    Function getStat() As String

        Return stat

    End Function


    'Returns item ID
    Function getID() As Integer

        Return ID

    End Function


End Class
