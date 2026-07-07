using SkiaSharp;

namespace Maanfee.Dashboard.Views.Core
{
    public static class CaptchaGenerator
    {
        public static (string ImageBase64, string CaptchaText) GenerateCaptcha()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var captchaText = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            int width = 200, height = 80;
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            // 1. خطوط اعوجاج (با SKPaint)
            using var distortPaint = new SKPaint
            {
                Color = SKColors.LightGray,
                StrokeWidth = 2,
                IsAntialias = true
            };
            for (int i = 0; i < 5; i++)
            {
                canvas.DrawLine(random.Next(width), random.Next(height),
                                random.Next(width), random.Next(height), distortPaint);
            }

            // 2. تنظیم فونت با SKFont
            using var font = new SKFont
            {
                Size = 36,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright)
            };

            // 3. کشیدن متن با SKFont و SKPaint
            using var textPaint = new SKPaint
            {
                Color = SKColors.DarkBlue,
                IsAntialias = true,
                //TextAlign = SKTextAlign.Center // توجه: TextAlign هنوز در SKPaint است
            };

            float x = 30;
            float y = height / 2f + 12;
            foreach (char c in captchaText)
            {
                canvas.Save();
                canvas.RotateDegrees(random.Next(-15, 15), x, y);
                canvas.DrawText(c.ToString(), x, y, SKTextAlign.Center, font, textPaint);
                canvas.Restore();
                x += 30;
            }

            // 4. نویز (نقاط تصادفی)
            using var noisePaint = new SKPaint { Color = SKColors.Gray };
            for (int i = 0; i < 200; i++)
            {
                canvas.DrawCircle(random.Next(width), random.Next(height), 1, noisePaint);
            }

            // 5. تبدیل به Base64
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            var base64 = Convert.ToBase64String(data.ToArray());

            return ($"data:image/png;base64,{base64}", captchaText);
        }
    }
}
