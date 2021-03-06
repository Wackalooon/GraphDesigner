﻿using System;
using System.Collections.Generic;
using System.Drawing;



namespace GraphDesigner
{
    [Serializable()]
    class NodeClass
    {
        private int nodeNumber;

        private Point nodePosition;

        public List<EdgeClass> nodeEdges;

        private const int sizeOfNode = 30;

        private const double radiusOfFreedom = 1.15;

        public int NodeNumber
        {
            get
            {
                return nodeNumber;
            }
            set
            {
                nodeNumber = value;
            }
        }

        public Point NodePosition
        {
            get
            {
                return nodePosition;
            }
            set
            {
                nodePosition = value;
            }
        }

        public NodeClass(Point positionOnMap, int number)
        {
            this.NodePosition = positionOnMap;
            this.nodeNumber = number;
            nodeEdges = new List<EdgeClass>();
        }

        public void addEdge(NodeClass nextNode)
        {
            int weight = calculateWeight(nextNode);
            EdgeClass newEdge = new EdgeClass(nextNode, weight);
            nodeEdges.Add(newEdge);
        }

        public void addEdge(EdgeClass newEdge)
        {
            int weight = calculateWeight(newEdge.NextNode);
            newEdge.Weight = weight;
            nodeEdges.Add(newEdge);
        }

        private int calculateWeight(NodeClass nextNode)
        {
            // weight is a distance between nodes in px
            int result = 0;
            double x = Math.Pow(Convert.ToDouble(this.nodePosition.X - nextNode.nodePosition.X), 2);
            double y = Math.Pow(Convert.ToDouble(this.nodePosition.Y - nextNode.nodePosition.Y), 2);
            result =  (int)Math.Floor(Math.Sqrt(x + y));

            return result;
        }

        public void drawNode(Graphics placeWhereShowIt, Color nodeColor, Color fillColor)
        {
            Rectangle rectangle = new Rectangle(this.nodePosition.X - sizeOfNode / 2,
                                                this.nodePosition.Y - sizeOfNode / 2,
                                                sizeOfNode, sizeOfNode);

            placeWhereShowIt.DrawEllipse(new Pen(nodeColor, 3), rectangle);

            placeWhereShowIt.FillEllipse(new SolidBrush(fillColor), rectangle);
            // Create string to draw.
            String drawString = this.nodeNumber.ToString();

            // Create font and brush.
            Font drawFont = new Font("Arial", 12);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF(this.nodePosition.X - 11, this.nodePosition.Y - 8);

            // Draw string to screen.
            placeWhereShowIt.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }

        public bool nodeWasClicked(Point click, bool withFreedomRadius)
        {
            // find distance between this node and click point
            int xMiss = this.nodePosition.X - click.X;
            int yMiss = this.nodePosition.Y - click.Y;
            xMiss = xMiss * xMiss; // too long expresion -> (int)Math.Pow(xMiss, 2);
            yMiss = yMiss * yMiss; //                    (int)Math.Pow(yMiss, 2);
            int result = (int)Math.Sqrt(xMiss + yMiss);

            // 15% of node's size is radius of freedom, it's forbidden to create new nodes in this area
            if (withFreedomRadius)
                return (result < sizeOfNode * radiusOfFreedom) ? true : false;
            else
                return (result < sizeOfNode) ? true : false;
        }


    }
}
