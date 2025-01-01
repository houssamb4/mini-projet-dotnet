public static class FormTransition
{
    public static async Task SlideIn(Form form, SlideDirection direction)
    {
        int start = (direction == SlideDirection.Left) ? -form.Width : form.Width;
        int end = form.Left;
        
        form.Left = start;
        form.Show();
        
        while(form.Left != end)
        {
            await Task.Delay(1);
            form.Left += (direction == SlideDirection.Left) ? 20 : -20;
            if((direction == SlideDirection.Left && form.Left > end) ||
               (direction == SlideDirection.Right && form.Left < end))
            {
                form.Left = end;
            }
        }
    }
    
    public enum SlideDirection
    {
        Left,
        Right
    }
}