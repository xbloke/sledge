﻿using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sledge.BspEditor.Documents;
using Sledge.BspEditor.Modification;
using Sledge.BspEditor.Modification.Operations;
using Sledge.BspEditor.Primitives.MapData;
using Sledge.BspEditor.Tools.Properties;
using Sledge.Common.Shell.Commands;
using Sledge.Common.Shell.Context;
using Sledge.Common.Shell.Menu;
using Sledge.Common.Translations;
using Sledge.Shell;

namespace Sledge.BspEditor.Tools.Texture
{
    [AutoTranslate]
    [Export(typeof(ICommand))]
    [CommandID("BspEditor:ReplaceTextures")]
    [MenuItem("Tools", "", "Texture", "B")]
    [MenuImage(typeof(Resources), nameof(Resources.Menu_ReplaceTextures))]
    public class ReplaceTextures : ICommand
    {
        [Import] private ITranslationStringProvider _translation;

        public string Name { get; set; } = "Replace textures...";
        public string Details { get; set; } = "Replace textures";

        public bool IsInContext(IContext context)
        {
            return context.TryGet("ActiveDocument", out MapDocument _);
        }

        public async Task Invoke(IContext context, CommandParameters parameters)
        {
            var md = context.Get<MapDocument>("ActiveDocument");
            if (md == null) return;
            using (var tb = new TextureReplaceDialog(md))
            {
                _translation.Translate(tb);
                if (await tb.ShowDialogAsync() == DialogResult.OK)
                {
                    var op = await tb.GetOperation();
                    if (op != null) MapDocumentOperation.Perform(md, op);
                }
            }
        }
    }
}
