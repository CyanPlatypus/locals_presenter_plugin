using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Msagl.Drawing;

namespace WindowVSIX.Logic
{
    class LocalsManager
    {
        public static Graph ManageLocals(DTE dte, Expressions exps)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int id = 1;


            List<Node> nodesToDraw = new List<Node>();
            Dictionary<string, Node> hashes = new Dictionary<string, Node>();

            foreach (Expression gr in exps)
            {
                LocalsManager.BuildNode(dte, null, nodesToDraw, hashes, gr, string.Empty, ref id, true);
            }

            return DrawManager.CreadeDrawableNoeds(nodesToDraw);
        }

        protected static void BuildNode(DTE dte, Node parent, List<Node> nodeList, Dictionary<string, Node> hashNodeDict, Expression ex
            , string howToCallExWithDot, ref int idCounter, bool isInLocal = false)
        {
            //check for null
            LinkedList<int> llll = new LinkedList<int>();
            if (dte.Debugger.GetExpression($"{howToCallExWithDot}{ex.Name}==null").Value == "true")
                return;
            //check for value type
            if (dte.Debugger.GetExpression($"{howToCallExWithDot}{ex.Name}.GetType().IsValueType").Value == "true")
                return;

            Expression hashExp = dte.Debugger.GetExpression($"{howToCallExWithDot}{ex.Name}.GetHashCode()");
            if (!hashExp.IsValidValue)
                return;
            string hash = hashExp.Value;

            if (!hashNodeDict.ContainsKey((hash)))
            {
                //string additionalInfo = dte.Debugger.GetExpression($"{howToCallExWithDot}{ex.Name}.GetType().Name").Value;
                string additionalInfo = ex.Type.Split('.').Last();
                //.Value.Substring(0, 20);
                if (additionalInfo.Length > 21)
                    additionalInfo = additionalInfo.Substring(0, 20);

                //looking for value type and string attributes
                //to include them into node Id
                List<string> infoStrings = new List<string>();
                foreach (Expression grDataMember in ex.DataMembers)
                {
                    if (grDataMember.Type == typeof(string).Name ||
                        grDataMember.Type == typeof(int).Name ||
                        grDataMember.Type == "int")
                    {
                        //stringBuilder.AppendLine($"({grDataMember.Name}) {grDataMember.Value}");
                        infoStrings.Add(grDataMember.Value);
                    }
                }

                Node node = new Node()
                {
                    Hash = hash,
                    Id = (idCounter++).ToString(),
                    Caption = ex.Name + (infoStrings.Count > 0 ? Environment.NewLine : string.Empty)
                                      //+ additionalInfo + Environment.NewLine 
                                      + string.Join(Environment.NewLine, infoStrings)
                };

                //var tmp = ex.Value;

                hashNodeDict.Add(hash, node);

                if (parent != null && !parent.ChildNodes.Exists(n => n.Hash == hash))
                    parent.ChildNodes.Add(node);
                if (parent == null)
                    nodeList.Add(node);

                foreach (Expression grDataMember in ex.DataMembers)
                {
                    BuildNode(dte, node, nodeList, hashNodeDict, grDataMember,
                        howToCallExWithDot + ex.Name + ".",
                        ref idCounter);

                }

            }
            else
            {
                Node existingNode = hashNodeDict[hash];
                if (existingNode != null)
                {
                    if (isInLocal)
                        existingNode.Caption = ex.Name + Environment.NewLine +
                            dte.Debugger.GetExpression($"{howToCallExWithDot}{ex.Name}.GetType().Name").Value;

                    if (parent != null && !parent.ChildNodes.Exists(n => n.Hash == hash))
                        parent.ChildNodes.Add(existingNode);
                }
            }
        }
    }

    public class Node
    {
        public string Caption { get; set; }
        public string Id { get; set; }
        public string Hash { get; set; }

        public bool IsNew { get; set; }

        public List<Node> ChildNodes { get; set; } = new List<Node>();
    }
}
