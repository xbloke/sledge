using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sledge.BspEditor.Rendering.Viewport
{
    public class RightClickMenuBuilder
    {
        public ViewportEvent Event { get; }
        public MapViewport Viewport { get; }
        public bool Intercepted { get; set; }
        private List<ToolStripItem> Items { get; }
        public bool IsEmpty => Items.Count == 0;

        public RightClickMenuBuilder(MapViewport viewport, ViewportEvent viewportEvent)
        {
            Event = viewportEvent;
            Viewport = viewport;
            Items = new List<ToolStripItem>
            {
                new CommandItem("BspEditor:Edit:Paste"),
                new CommandItem("BspEditor:Edit:PasteSpecial"),
                new ToolStripSeparator(),
                new CommandItem("BspEditor:Edit:Undo"),
                new CommandItem("BspEditor:Edit:Redo")
            };
        }

        public ToolStripMenuItem AddCommand(string commandId, object parameters = null)
        {
            var mi = new CommandItem(commandId, parameters);
            Items.Add(mi);
            return mi;
        }

        public ToolStripMenuItem AddCallback(string description, Action callback)
        {
            var mi = new ToolStripMenuItem(description);
            mi.Click += (s, e) => callback();
            Items.Add(mi);
            return mi;
        }

        public ToolStripSeparator AddSeparator()
        {
            var mi = new ToolStripSeparator();
            Items.Add(mi);
            return mi;
        }

        public ToolStripMenuItem AddGroup(string description)
        {
            var g = new ToolStripMenuItem(description);
            Items.Add(g);
            return g;
        }

        public void Add(params ToolStripItem[] items)
        {
            Items.AddRange(items);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public void Populate(ContextMenuStrip menu)
        {
            menu.Items.Clear();
            foreach (var command in Items)
            {
                menu.Items.Add(command);
            }
        }

        private class CommandItem : ToolStripMenuItem
        {
            private readonly string _commandID;
            private readonly object _parameters;

            public CommandItem(string commandID, object parameters = null)
            {
                _commandID = commandID;
                _parameters = parameters;
                Text = commandID;
            }
        }
    }
}