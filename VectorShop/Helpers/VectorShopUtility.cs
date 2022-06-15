using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace VectorShop.Helpers
{
    public static class VectorShopUtility
    {

        public static string StringDasher(string input)
        {
            return input.Replace(" ", "-");
        } //turning spaced test to dashed test, useful for url encoding.

        public static string PullNameFromEmail(string input)
        {
            string[] st = input.Split('@');
            return st[0];
        }
        public static string TrimFrontIfLongerThan(this string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }//take a string and trim the rest

        public static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static string SplitOnDot(this string value)
        {
            string striped = StripHtml(value);
            return striped.Split('.').First() + ".";
        }

        #region Batching, take a certain number of collection and return it each time through the loop
        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }
        #endregion

        public static bool IsImgDimAcceptable(HttpPostedFileBase file, int maxHeight, int maxWidth)
        {
            Image img = Image.FromStream(file.InputStream, true, true);

            if (img.Height > maxHeight || img.Width > maxWidth)
            {
                return false;
            }

            return true;
        }
        public static bool IsImgDimAcceptable(HttpPostedFileBase file, int minHeight, int minWidth, int maxHeight, int maxWidth)
        {
            Image img = Image.FromStream(file.InputStream, true, true);

            if (img.Height < minHeight || img.Width < minWidth)
            {
                return false;
            }

            if (img.Height > maxHeight || img.Width > maxWidth)
            {
                return false;
            }

            return true;
        }
        public static Image ScaleImage(HttpPostedFileBase file, int maxWidth, int maxHeight)
        {
            Image image = Image.FromStream(file.InputStream, true, true);

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}