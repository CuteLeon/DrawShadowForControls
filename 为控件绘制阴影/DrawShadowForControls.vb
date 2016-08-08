Module DrawShadowForControls
    '初始的过程参数
    Public ShadowSize As Integer = 5                     '阴影大小
    Public ShadowDistance As Integer = 15             '阴影距离
    Public ShadowAngle As Integer = 120               '阴影角度
    Public ShadowColor As Color = Color.Black       '阴影颜色
    Public ShadowOpacity As Single = 0.75             '阴影不透明度

    '窗体调用
    Public Sub ShadowMe(ByVal ShadowWindow As Form)
        Dim ShadowBitmap As New Bitmap(ShadowWindow.Width, ShadowWindow.Height)
        Dim ShadowLeft, ShadowTop As Integer

        '把所有控件的阴影合并到ShadownBitmap
        For Each MyControl In ShadowWindow.Controls
            ShadowLeft = MyControl.Left - ShadowSize - Math.Cos(2 * Math.PI * ShadowAngle / 360) * ShadowDistance
            ShadowTop = MyControl.Top - ShadowSize + Math.Sin(2 * Math.PI * ShadowAngle / 360) * ShadowDistance
            CombineBitmap(ShadowBitmap, CreatShadow(MyControl, ShadowSize, ShadowColor), ShadowLeft, ShadowTop)
        Next

        '把ShadowBitmap绘制到窗体背景
        Dim MyGraphics As Graphics = Graphics.FromImage(ShadowWindow.BackgroundImage)
        MyGraphics.DrawImage(ShadowBitmap, 0, 0)
    End Sub

    '创建阴影位图
    Private Function CreatShadow(ByVal ShadowControl As Control, ByVal ShadowSize As Integer, ByVal ShadowColor As Color, Optional ByVal ShadowOpacity As Single = 0.75) As Bitmap
        Dim DoubleSize As Integer = ShadowSize * 2
        Dim ShadowBitmap As New Bitmap(ShadowControl.Width + DoubleSize, ShadowControl.Height + DoubleSize)
        Dim ShadowX, ShadowY As Integer
        Dim Distance As Integer
        Dim RightBorder = (ShadowBitmap.Width - 1) \ 2
        Dim DownBorder = (ShadowBitmap.Height - 1) \ 2 '
        Dim ForX As Integer = DoubleSize, ForY As Integer = DoubleSize

        If DoubleSize > DownBorder Then ForY = DownBorder
        If DoubleSize > RightBorder Then ForX = RightBorder

        For ShadowX = DoubleSize To RightBorder
            For ShadowY = 0 To ForY
                ShadowBitmap.SetPixel(ShadowX, ShadowY, Color.FromArgb(255 * ShadowOpacity * ShadowY ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowY, Color.FromArgb(255 * ShadowOpacity * ShadowY ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowX, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * ShadowY ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * ShadowY ^ 2 / DoubleSize ^ 2, ShadowColor))
            Next
        Next

        For ShadowX = 0 To ForX
            For ShadowY = DoubleSize To DownBorder
                ShadowBitmap.SetPixel(ShadowX, ShadowY, Color.FromArgb(255 * ShadowOpacity * ShadowX ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowY, Color.FromArgb(255 * ShadowOpacity * ShadowX ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowX, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * ShadowX ^ 2 / DoubleSize ^ 2, ShadowColor))
                ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * ShadowX ^ 2 / DoubleSize ^ 2, ShadowColor))
            Next
        Next

        For ShadowX = 0 To ForX
            For ShadowY = 0 To ForY
                Distance = DoubleSize - Math.Round(Math.Sqrt((DoubleSize - ShadowX) ^ 2 + (DoubleSize - ShadowY) ^ 2))
                If Distance > -1 Then
                    ShadowBitmap.SetPixel(ShadowX, ShadowY, Color.FromArgb(255 * ShadowOpacity * Distance ^ 2 / DoubleSize ^ 2, ShadowColor)) '左上
                    ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowY, Color.FromArgb(255 * ShadowOpacity * Distance ^ 2 / DoubleSize ^ 2, ShadowColor)) '右上
                    ShadowBitmap.SetPixel(ShadowX, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * Distance ^ 2 / DoubleSize ^ 2, ShadowColor)) '左下
                    ShadowBitmap.SetPixel(ShadowBitmap.Width - ShadowX - 1, ShadowBitmap.Height - ShadowY - 1, Color.FromArgb(255 * ShadowOpacity * Distance ^ 2 / DoubleSize ^ 2, ShadowColor)) '右下
                End If
            Next
        Next

        If Not (ForX = RightBorder Or ForY = DownBorder) Then
            For ShadowX = DoubleSize To ShadowBitmap.Width - DoubleSize - 1
                For ShadowY = DoubleSize To ShadowBitmap.Height - DoubleSize - 1
                    ShadowBitmap.SetPixel(ShadowX, ShadowY, Color.FromArgb(ShadowColor.A * ShadowOpacity, ShadowColor))
                Next
            Next
        End If

        Return ShadowBitmap
    End Function

    '复制单个控件的ShadownBitmap到整个窗体的ShadowBitmap里
    Private Sub CombineBitmap(ByVal PaneBitmap As Bitmap, ByVal CopyBitmap As Bitmap, ByVal BitmapX As Integer, ByVal BitmapY As Integer)
        Dim MyGraphics As Graphics = Graphics.FromImage(PaneBitmap)
        MyGraphics.DrawImage(CopyBitmap, BitmapX, BitmapY)
    End Sub
End Module
