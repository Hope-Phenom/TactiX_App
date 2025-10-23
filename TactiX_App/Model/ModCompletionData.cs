using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TactiX_Models.Tactics;
using TactiX_ModSupport;

namespace TactiX_App.Model
{
    public class ModCompletionData : ICompletionData
    {
        public IImage? Image { get; }
        public string Text { get; }
        public object Content { get; }
        public object Description { get; }
        public double Priority => 1.0;

        private const string ICON_FOLDER = "icons";

        private readonly ISegment? _segment;

        public ModCompletionData(ModResourceCache<Bitmap>? modResourceCache, ISegment? segment, L_ModItem modItem)
        {
            Image = modResourceCache == null
                ? null
                : modResourceCache.GetImage(Path.Combine(ICON_FOLDER, $"{modItem.Abbr}.png"));
            Text = modItem.Abbr;
            Content = modItem.Abbr;
            Description = modItem.Desc;

            _segment = segment;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(_segment, Text);
        }
    }
}
