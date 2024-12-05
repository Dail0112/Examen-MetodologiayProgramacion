using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PracticaMetodologiaP
{
    public partial class Form1 : Form
    {
        public class Estudiante
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string NumeroEstudiante { get; set; }
            public string Carrera { get; set; }
            public double PromedioGeneral { get; set; }
            public int MateriasAprobadas { get; set; }


        }

        List<Estudiante> estudiantes = new List<Estudiante>(); 

        public Form1()
        {
            InitializeComponent();
            dgvEstudiantes.ContextMenuStrip = contextMenuStrip1; 

        }
        private void ActualizarEstado()
        {
            statusStripLabel.Text = $"Total de estudiantes: {estudiantes.Count}";
        }


        private void ActualizarDataGridView()
        {
            dgvEstudiantes.DataSource = null;
            dgvEstudiantes.DataSource = estudiantes;
        }

        private bool ValidarDatos()
        {
            // Validar que el promedio sea un número válido y no negativo
            if (!double.TryParse(txtPromedio.Text, out double promedio) || promedio < 0)
            {
                MessageBox.Show("El promedio debe ser un número Positivo.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPromedio.Focus();
                return false;
            }

            // Validar que el número de materias sea un número entero válido y no negativo
            if (!int.TryParse(txtMatAprob.Text, out int materiasAprobadas) || materiasAprobadas < 0)
            {
                MessageBox.Show("El número de materias aprobadas debe ser un número Positivo.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatAprob.Focus();
                return false;
            }

            // Validar campos vacíos 
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtNumEstudiante.Text) || cmbCarrera.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos deben estar completos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tpDetalles_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                Estudiante nuevoEstudiante = new Estudiante
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    NumeroEstudiante = txtNumEstudiante.Text,
                    Carrera = cmbCarrera.SelectedItem.ToString(),
                    PromedioGeneral = double.Parse(txtPromedio.Text),
                    MateriasAprobadas = int.Parse(txtMatAprob.Text)
                };
                estudiantes.Add(nuevoEstudiante);
                ActualizarVista();
                ActualizarGrafico();
                ActualizarEstado();
                MessageBox.Show("Estudiante registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActualizarVista()
        {
            dgvEstudiantes.DataSource = null;
            dgvEstudiantes.DataSource = estudiantes;
            statusStripLabel.Text = $"Total de estudiantes: {estudiantes.Count}";
        }

        private void ActualizarGrafico()
        {
            chartEstudiantes.Series.Clear();
            var carrerasAgrupadas = estudiantes.GroupBy(e => e.Carrera)
                                               .Select(g => new { Carrera = g.Key, Cantidad = g.Count() });

            Series serie = new Series("Estudiantes por Carrera");
            foreach (var carrera in carrerasAgrupadas)
            {
                serie.Points.AddXY(carrera.Carrera, carrera.Cantidad);
            }
            chartEstudiantes.Series.Add(serie);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizarEstado();
        }

        private void txtEdad_TextChanged(object sender, EventArgs e)
        {

        }

        private void chbBeca_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtNomyApll_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbCarrera_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count > 0)
            {
                // Obtener el índice del estudiante seleccionado
                int indice = dgvEstudiantes.SelectedRows[0].Index;

                // Confirmación antes de eliminar
                DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este estudiante?",
                                                         "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultado == DialogResult.Yes)
                {
                    estudiantes.RemoveAt(indice); // Eliminar de la lista
                    ActualizarVista(); // Refrescar la vista
                    ActualizarGrafico(); // Refrescar el gráfico
                    ActualizarEstado();
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un estudiante para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarVista();   // Refresca la lista del DataGridView
            ActualizarGrafico(); // Refresca el gráfico de distribución por carrera
            ActualizarEstado();
            MessageBox.Show("La lista y los gráficos se han actualizado.", "Actualización Completa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnEliminar_Click(sender, e); 
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEstudiantes.SelectedRows.Count > 0)
            {
                int indice = dgvEstudiantes.SelectedRows[0].Index;
                Estudiante estudiante = estudiantes[indice];

                
                txtNombre.Text = estudiante.Nombre;
                txtApellido.Text = estudiante.Apellido;
                txtNumEstudiante.Text = estudiante.NumeroEstudiante;
                cmbCarrera.SelectedItem = estudiante.Carrera;
                txtPromedio.Text = estudiante.PromedioGeneral.ToString();
                txtMatAprob.Text = estudiante.MateriasAprobadas.ToString();

                
                estudiantes.RemoveAt(indice);
                ActualizarVista();
            }
        }
    }
}
