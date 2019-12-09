using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Epichurco.Benj8RIDES.Formatos
{
    class TopBottomTableBorderMaker : IPdfPTableEvent
    {
        private BaseColor _borderColor;
        private float _borderWidth;

        /// <summary>
        /// Add a top and bottom border to the table.
        /// </summary>
        /// <param name="borderColor">The color of the border.</param>
        /// <param name="borderWidth">The width of the border</param>
        public TopBottomTableBorderMaker(BaseColor borderColor, float borderWidth)
        {
            this._borderColor = borderColor;
            this._borderWidth = borderWidth;
        }
        public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        {
            //widths (should be thought of as x's) is an array of arrays, first index is for each row, second index is for each column
            //The below uses first and last to calculate where each X should start and end
            var firstRowWidths = widths[0];
            var lastRowWidths = widths[widths.Length - 1];

            var firstRowXStart = firstRowWidths[0];
            var firstRowXEnd = firstRowWidths[firstRowWidths.Length - 1] - firstRowXStart;

            var lastRowXStart = lastRowWidths[0];
            var lastRowXEnd = lastRowWidths[lastRowWidths.Length - 1] - lastRowXStart;

            //heights (should be thought of as y's) is the y for each row's top plus one extra for the last row's bottom
            //The below uses first and last to calculate where each Y should start and end
            var firstRowYStart = heights[0];
            var firstRowYEnd = heights[1] - firstRowYStart;

            var lastRowYStart = heights[heights.Length - 1];
            var lastRowYEnd = heights[heights.Length - 2] - lastRowYStart;

            //Where we're going to draw our lines
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];

            //I always try to save the previous state before changinge anything
            canvas.SaveState();

            //Set our line properties
            canvas.SetLineWidth(this._borderWidth);
            canvas.SetColorStroke(this._borderColor);

            //Draw some rectangles
            canvas.Rectangle(
                            firstRowXStart,
                            firstRowYStart,
                            firstRowXEnd,
                            firstRowYEnd
                            );
            //They aren't actually drawn until you stroke them!
            canvas.Stroke();

            canvas.Rectangle(
                            lastRowXStart,
                            lastRowYStart,
                            lastRowXEnd,
                            lastRowYEnd
                            );
            canvas.Stroke();

            //Restore any previous settings
            canvas.RestoreState();

        }
    }
}
