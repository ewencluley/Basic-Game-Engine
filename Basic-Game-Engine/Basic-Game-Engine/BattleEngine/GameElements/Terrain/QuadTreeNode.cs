using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleEngine.GameElements.Terrain
{
    class QuadTreeNode
    {
        QuadTreeNode parent;
        QuadTree parentTree;
        int positionIndex;

        int nodeDepth, nodeSize;

        #region VERTICES
        public QuadNodeVertex VertexTopLeft;
        public QuadNodeVertex VertexTop;
        public QuadNodeVertex VertexTopRight;
        public QuadNodeVertex VertexLeft;
        public QuadNodeVertex VertexCenter;
        public QuadNodeVertex VertexRight;
        public QuadNodeVertex VertexBottomLeft;
        public QuadNodeVertex VertexBottom;
        public QuadNodeVertex VertexBottomRight;
        #endregion

        #region CHILDREN
        public QuadTreeNode ChildTopLeft;
        public QuadTreeNode ChildTopRight;
        public QuadTreeNode ChildBottomLeft;
        public QuadTreeNode ChildBottomRight;
        #endregion

        #region NEIGHBORS
        public QuadTreeNode NeighborTop;
        public QuadTreeNode NeighborRight;
        public QuadTreeNode NeighborBottom;
        public QuadTreeNode NeighborLeft;
        #endregion

        public BoundingBox Bounds;
        public NodeType NodeType;

        /// <summary>
        /// QuadNode constructor
        /// </summary>
        /// <param name="nodeType">Type of node.</param>
        /// <param name="nodeSize">Width/Height of node (# of vertices across - 1).</param>
        /// <param name="nodeDepth">Depth of current node</param>
        /// <param name="parent">Parent QuadNode</param>
        /// <param name="parentTree">Top level Tree.</param>
        /// <param name="positionIndex">Index of top left Vertice in the parent tree Vertices array</param>
        public QuadTreeNode(NodeType nodeType, int nodeSize, int nodeDepth, QuadTreeNode parent, QuadTree parentTree, int positionIndex)
        {
            NodeType = nodeType;
            this.nodeSize = nodeSize;
            this.nodeDepth = nodeDepth;
            this.positionIndex = positionIndex;
 
            this.parent = parent;
            this.parentTree = parentTree;
     
            //Add the 9 vertices
            //AddVertices();
 
            //Bounds = new BoundingBox(this.parentTree.Vertices[VertexTopLeft.Index].Position, 
                        //this.parentTree.Vertices[VertexBottomRight.Index].Position);
            Bounds.Min.Y = -950f;
            Bounds.Max.Y = 950f;
 
            if (nodeSize >= 4)
                //AddChildren();
 
            //Make call to UpdateNeighbors from the parent node.
            //This will update all neighbors recursively for the
            //children as well.  This ensures all nodes are created 
            //prior to updating neighbors.
            if (this.nodeDepth == 1)
            {
                //AddNeighbors();
 
                VertexTopLeft.Activated = true;
                VertexTopRight.Activated = true;
                VertexCenter.Activated = true;
                VertexBottomLeft.Activated = true;
                VertexBottomRight.Activated = true;
 
            }
        }
    



        /// <summary>
        /// Checks whether the node is a leaf node or not, i.e. whether it has children
        /// </summary>
        /// <returns>bool specifying whether the node has children</returns>
        public bool HasChildren()
        {
            if (ChildTopLeft != null && ChildTopRight != null && ChildBottomLeft != null && ChildBottomRight != null) //if all of the children are not null then
            {
                return true; //the node has children
            }
            else //otherwise
            {
                return false; //the node does not have children
            }
        }
    }
}
