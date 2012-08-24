using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleEngine.GameElements.Terrain
{

    public enum NodeType
    {
        FullNode = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4
    }

    public struct QuadNodeVertex
    {
        public int Index;
        public bool Activated;
    }

    public class QuadTree
    {

        private QuadTreeNode _rootNode;
        private TreeVertexCollection _vertices;
        private BufferManager _buffers;
        private Vector3 _position;
        private int _topNodeSize;

        private Vector3 _cameraPosition;
        private Vector3 _lastCameraPosition;

        public int[] Indices;

        public Matrix View;
        public Matrix Projection;

        public GraphicsDevice Device;

        public int TopNodeSize { get { return _topNodeSize; } }
        //public QuadTreeNode RootNode { get { return _rootNode; } }
       // public TreeVertexCollection Vertices { get { return _vertices; } }
        public Vector3 CameraPosition
        {
            get { return _cameraPosition; }
            set { _cameraPosition = value; }
        }

        internal BoundingFrustum ViewFrustrum { get; set; }
    }
}
