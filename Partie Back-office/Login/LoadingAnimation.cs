using System;
using System.Drawing;
using System.Windows.Forms;

public class LoadingAnimation : Form
{
    private Timer timer;
    private int angle = 0;
    
    public LoadingAnimation()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.BackColor = Color.FromArgb(45, 52, 54);
        this.TransparencyKey = this.BackColor;
        this.Size = new Size(100, 100);
        this.StartPosition = FormStartPosition.CenterScreen;
        
        timer = new Timer();
        timer.Interval = 30;
        timer.Tick += Timer_Tick;
        timer.Start();
    }
    
    private void Timer_Tick(object sender, EventArgs e)
    {
        angle += 10;
        if(angle >= 360) angle = 0;
        this.Invalidate();
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using(Pen pen = new Pen(Color.FromArgb(85, 239, 196), 4))
        {
            Rectangle rect = new Rectangle(10, 10, this.Width - 20, this.Height - 20);
            int startAngle = angle;
            e.Graphics.DrawArc(pen, rect, startAngle, 300);
        }
    }
} 