		void SeleCell(Int32 RowIndex)
        {
            if (RowIndex < 0 || RowIndex >= DGVDatos.RowCount) return;

            var Cell = DGVDatos.Rows[RowIndex].Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);

            if (Cell != null)
            {
                DGVDatos.CurrentCell = Cell;
                DGVDatos.ClearSelection();
                Cell.Selected = true;
                LoadLabel();
            }

        }

        void LoadLabel()
        {
            Int32 TotalFilas = DGVDatos.RowCount;
            Int32 FilaActual = DGVDatos.CurrentCell?.RowIndex ?? -1;

            if (FilaActual >= 0 && FilaActual < TotalFilas)
            {
                LabFoto.Text = $"{FilaActual + 1}/{TotalFilas}";
            }
            else
            {
                LabFoto.Text = $"{TotalFilas}";
            }
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (DGVDatos.RowCount == 0) return;

            Int32 CurrentRow = DGVDatos.CurrentCell?.RowIndex ?? 0;

            Int32 NewRow = CurrentRow - 1;

            if (NewRow < 0)
            {
                NewRow = DGVDatos.RowCount - 1;
            }

            SeleCell(NewRow);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (DGVDatos.Rows.Count == 0) return;

            Int32 CurrentRow = DGVDatos.CurrentCell?.RowIndex ?? -1;

            Int32 NewRow = CurrentRow + 1;

            if (NewRow >= DGVDatos.Rows.Count) NewRow = 0;

            SeleCell(NewRow);
        }