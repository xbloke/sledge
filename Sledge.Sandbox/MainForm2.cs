using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Sledge.DataStructures.Geometric;
using Sledge.DataStructures.MapObjects;
using Sledge.Editor.Brushes;
using Sledge.Rendering;
using Sledge.Rendering.Cameras;
using Sledge.Rendering.DataStructures;
using Sledge.Rendering.Materials;
using Sledge.Rendering.OpenGL;

namespace Sledge.Sandbox
{
    public class MainForm2 : Form
    {
        public MainForm2()
        {
            ClientSize = new Size(600, 600);

            // Test out the octree
            var random = new Random();
            var oct = new Octree<IOrigin>();
            oct.Add(Enumerable.Range(0, 50000).Select(x => new Sledge.Rendering.Line() { Origin = new Coordinate(random.Next(-9999, 9999), random.Next(-9999, 9999), random.Next(-9999, 9999)) }));
            oct.Add(Enumerable.Range(0, 50000).Select(x => new Sledge.Rendering.Line() { Origin = new Coordinate(x % 100, x % 100, x % 100) }));
            var groups = oct.Partition();

            // Create engine
            var renderer = new OpenGLRenderer();
            var engine = new Engine(renderer);

            // Get render control/context
            var camera = new PerspectiveCamera { Position = new Coordinate(-10, -10, -10), LookAt = Coordinate.Zero };
            //var camera = new OrthographicCamera() { Zoom = 16 };
            var viewport = engine.CreateViewport(camera);

            viewport.Control.Dock = DockStyle.Fill;
            this.Controls.Add(viewport.Control);

            // Create scene
            var scene = engine.Scene;

            var light = new AmbientLight(Color.White, new Coordinate(1, 2, 3), 0.8f);
            scene.Add(light);

            var material = Material.Flat(Color.LightGreen);

            var b = new BlockBrush();
            var brushes = b.Create(new IDGenerator(), new Box(-Coordinate.One * 3, Coordinate.One * 2), null, 2).ToList();
            foreach (var s in brushes.OfType<Solid>().SelectMany(x => x.Faces))
            {
                var face = new Rendering.Face(material, s.Vertices.Select(x => new Sledge.Rendering.Vertex(x.Location, x.TextureU, x.TextureV)).ToList());
                scene.Add(face);
            }

            // Add scene to renderer / add renderer to scene


        }
    }
}