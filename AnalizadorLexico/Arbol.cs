using System.Collections.Generic;
using System.Windows.Forms;

namespace AnalizadorLexico
{
    public class Arbol
    {
        public string Nombre { get; set; }
        public List<Arbol> Hijos { get; set; }

        public Arbol(string nombre)
        {
            Nombre = nombre;
            Hijos = new List<Arbol>();
        }

        public TreeNode ToTreeNode()
        {
            TreeNode nodo = new TreeNode(Nombre);
            foreach (var hijo in Hijos)
            {
                nodo.Nodes.Add(hijo.ToTreeNode());
            }
            return nodo;
        }
    }
}
