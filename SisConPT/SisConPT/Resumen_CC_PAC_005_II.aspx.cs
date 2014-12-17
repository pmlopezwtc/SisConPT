﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace SisConPT.SisConPT
{
    public partial class Resumen_CC_PAC_005_II : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            System.Configuration.ConnectionStringSettings connStringLM;
            if (Session["PlantaName"] != null)
            {
                connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
                string PlantaNombre = Session["PlantaName"].ToString();
                string comando = "SELECT convert(varchar(10),placodigo) as placodigo FROM planta WHERE pladescri ='" + PlantaNombre + "'";
                SqlConnection conexion = new SqlConnection(connStringmain.ToString());
                conexion.Open();
                SqlCommand sql = new SqlCommand(comando, conexion);
               
                using (SqlDataReader reader = sql.ExecuteReader())
                {
                    reader.Read();
                    txt_cod_plan.Text = reader.GetString(0);
                    }
                conexion.Close();

            }
            if (Session["PlantaName"].ToString() == "Planta Mostazal")
            {
                connStringLM = rootWebConfig.ConnectionStrings.ConnectionStrings["LotManager01"];

            }
            else
            {
                connStringLM = rootWebConfig.ConnectionStrings.ConnectionStrings["LotManager40"];

            }

            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            SqlConnection conexion_2 = new SqlConnection(connStringmain.ToString());
            conexion_2.Open();
            SqlCommand cmd_proc = new SqlCommand("select * from controlpt where placodigo ='" + txt_cod_plan.Text + "'", conexion_2);
            SqlDataAdapter sda_proc = new SqlDataAdapter(cmd_proc);
            DataSet ds_proc = new DataSet();
            try
            {
                sda_proc.Fill(ds_proc);
                conexion_2.Close();
                if (ds_proc.Tables[0].Rows.Count.ToString() == "0")
                {
                    string error = "Sin informacion para mostrar";
                    Response.Write("<script language=javascript > alert('" + error + "'); </script>");
                    Exportar_005.Enabled = false;
                }

            }
            catch { }

            if (!IsPostBack)
            {

                BuscaTurno(); 

            }

                 
        }

        protected void linea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string linea = Convert.ToString(drop_linea_d.SelectedValue);

           // BuscaTurno(linea);

        }

        protected void turno_SelectedIndexChanged(object sender, EventArgs e)
        {
            string turno = Convert.ToString(drop_turno_d.SelectedValue);
            DropLinea(turno);

        }
     
        private void InitializeEditPopUp()
        {


        }

        protected void Procesos_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GridViewRow row = gvProcesos.Rows[e.NewSelectedIndex];

            string proceso = row.Cells[1].Text;
            string lote = row.Cells[2].Text;
            string destino = row.Cells[3].Text;
            string turno = Convert.ToString(drop_turno_d.SelectedValue);
            string linea_2 = Convert.ToString(drop_linea_d.SelectedValue);


            InitializeEditPopUp();
            PopUpDetalle(proceso, lote, destino);
            gv_solubles(proceso, lote, turno, linea_2);

            mpeEditOrder.Show();
        }

        protected void Procesos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProcesos.PageIndex = e.NewPageIndex;
            int linea_2 = Convert.ToInt32(drop_linea_d.SelectedValue);
        }

        private void PopUpDetalle(string proceso, string lote, string destino)
        {

            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());
            con.Open();
            string turno = Convert.ToString(drop_turno_d.SelectedValue);
            string linea_2 = Convert.ToString(drop_linea_d.SelectedValue);

            string inicio = txt_fechainicio.Text;
            string fin = txt_fechafin.Text;
            int planta = Convert.ToInt32(txt_cod_plan.Text);

            if (destino == "&nbsp;") { destino = ""; }
            
            string cadena_consulta = "";
            if (linea_2 == "Todas")
            {
                cadena_consulta = "[RESUMEN_CC_PAC_005_todas] '" + inicio + "','" + fin + "', '" + turno + "'," + planta + "; " +
            " select * from ##a where cptproces='" + proceso + "' and cptnulote='" + lote + "' and cptdestino='" + destino + "';";
            }
            else
            {
                cadena_consulta = "[RESUMEN_CC_PAC_005] '" + inicio + "','" + fin + "', '" + turno + "'," + linea_2 + "," + planta + "; " +
            " select * from ##a where cptproces='" + proceso + "' and cptnulote='" + lote + "' and cptdestino='" + destino + "';";
            }


            SqlCommand cmd_proc = new SqlCommand(cadena_consulta, con);
            try
            {

            using (SqlDataReader reader = cmd_proc.ExecuteReader())
            {
                reader.Read();
                lbl_proceso.Text = reader.GetString(0);
                lbl_productor.Text = reader.GetString(1);
                lbl_lote.Text = reader.GetString(2);

                txtprecalibre.Text = reader.GetString(5);
                txtdanotrip.Text = reader.GetString(6);
                txtescama.Text = reader.GetString(7);
                txtfrutosdeformes.Text = reader.GetString(8);
                txtfrutosdobles.Text = reader.GetString(9);
                txtguatablanca.Text = reader.GetString(10);
                txtherida.Text = reader.GetString(11);
                txtmanchas.Text = reader.GetString(12);
                txtmedialuna.Text = reader.GetString(13);
                txtpiellagarto.Text = reader.GetString(14);
                txtrusset.Text = reader.GetString(15);
                txtsutura.Text = reader.GetString(16);
                txtfaltocolor.Text = reader.GetString(17);
                txtramaleo.Text = reader.GetString(18);
                txtsinpedicelo.Text = reader.GetString(19);
                txtadhesion.Text = reader.GetString(20);
                txtdeshid.Text = reader.GetString(21);
                txtdeshidpedi.Text = reader.GetString(22);
                txtblandos.Text = reader.GetString(23);
                txtheridasabiertas.Text = reader.GetString(24);
                txtmachucon.Text = reader.GetString(25);
                txtpartiduras.Text = reader.GetString(26);
                txtpartidurasagua.Text = reader.GetString(27);
                txtpartiduracicatrizada.Text = reader.GetString(28);
                txtpitting.Text = reader.GetString(29);
                txtpudricion.Text = reader.GetString(30);
                txtmanchaspardas.Text = reader.GetString(31);
                txtdanopajaro.Text = reader.GetString(32);
                txtdesgarro.Text = reader.GetString(33);
                txtcortesierra.Text = reader.GetString(34);
                txt_sut_exp.Text = reader.GetString(35);
                lbl_variedad.Text = reader.GetString(40);
                lbl_clasi.Text  = reader.GetString(37);
                lbl_destino.Text  = reader.GetString(38);
                lbl_vaciadas.Text  = reader.GetString(39);
                lbl_casos.Text = reader.GetString(41);
           }

            con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            mpeEditOrder.Hide();
        }

        private void DropLinea(string turno)
        {
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());
            //try
            //{
            con.Open();
            //linea

            SqlCommand cmd_linea = new SqlCommand("select 'Todas' as lincodigo union select distinct lincodigo from controlpt where turcodigo='" + turno + "' and placodigo = '" + txt_cod_plan.Text + "'", con);
            SqlDataAdapter sda_linea = new SqlDataAdapter(cmd_linea);
            DataSet ds_linea = new DataSet();
            sda_linea.Fill(ds_linea);
            drop_linea_d.DataSourceID = "";
            drop_linea_d.DataSource = ds_linea;
            drop_linea_d.DataBind();

            con.Close();
        
        }

        private void BuscaTurno()
        {
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());
            con.Open();
            //linea
            
            SqlCommand cmd_linea = new SqlCommand("select distinct turcodigo from controlpt where placodigo = '" + txt_cod_plan.Text + "'", con);
            SqlDataAdapter sda_linea = new SqlDataAdapter(cmd_linea);
            DataSet ds_linea = new DataSet();
            sda_linea.Fill(ds_linea);

            try
            { 
            drop_turno_d.DataSourceID = "";
            drop_turno_d.DataSource = ds_linea;
            drop_turno_d.DataBind();

            string turno = "";
                if (drop_turno_d.Items.Count != 0)
                {
                    turno = Convert.ToString(drop_turno_d.SelectedValue);

                    DropLinea(turno);

                }
                if (drop_turno_d.Items.Count == 0)
                {

                    DropLinea(turno);

                }


            con.Close();
            }
            catch 
            {
                drop_turno_d.DataSourceID = "";
                drop_turno_d.DataSource = "";
                drop_turno_d.DataBind();

            }

            
        }

        private void GvProcesos_Llenar(string turno, string linea_2, string inicio, string fin)
        {

            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());
            con.Open();
            int planta = Convert.ToInt32(txt_cod_plan.Text);
            string comando_cadena = "";
            if (linea_2 == "Todas")
            {
                comando_cadena = "[RESUMEN_CC_PAC_005_todas] '" + inicio + "','" + fin + "', '" + turno + "'," + planta + "; select * from ##a;";
            }
            else
            {
                comando_cadena = "[RESUMEN_CC_PAC_005] '" + inicio + "','" + fin + "', '" + turno + "'," + linea_2 + "," + planta + "; select * from ##a;";
            }
            SqlCommand cmd_proc = new SqlCommand(comando_cadena, con);
            SqlDataAdapter sda_proc = new SqlDataAdapter(cmd_proc);
            DataSet ds_proc = new DataSet();
            try {
            sda_proc.Fill(ds_proc);
            gvProcesos.DataSource = ds_proc;
            gvProcesos.DataBind();


            con.Close();

            }
            catch (Exception e)
            {
                this.Page.Response.Write("<script language='JavaScript'>window.alert('" + e + "');</script>");
            }
        }

        private void gv_solubles(string proceso, string lote, string turno, string linea_2)
        {

            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());
            con.Open();
            string filtro = "";
            if (linea_2 == "Todas")
            {
                filtro = " where nroproceso='" + proceso + "' and nrolote='" + lote + "' and turno='" + turno + "' and placodigo= '" + txt_cod_plan.Text + "'";
            }
            else
            {
                filtro = " where nroproceso='" + proceso + "' and nrolote='" + lote + "' and turno='" + turno + "' and nrolinea='" + linea_2 + "' and placodigo= '" + txt_cod_plan.Text + "'";
            }

            string comando_cadena = "select codcaja, calibresoluble,convert(varchar (255),f1) as f1,convert(varchar (255),f2) as f2," +
            " convert(varchar (255),f3) as f3,convert(varchar (255),f4) as f4,convert(varchar (255),f5) as f5, convert(varchar(255)," +
            " (CONVERT(decimal(18, 2),(f1+f2+f3+f4+f5)/5.0))) as promedio from solidossolubles " +
            " as sol inner join controlpt as cl on sol.codcaja=cl.cptcodcja" + filtro;

            SqlCommand cmd_proc = new SqlCommand(comando_cadena, con);
            SqlDataAdapter sda_proc = new SqlDataAdapter(cmd_proc);
            DataSet ds_proc = new DataSet();
            try
            {
                sda_proc.Fill(ds_proc);
                gv_solubles_datos.DataSource = ds_proc;
                gv_solubles_datos.DataBind();


                con.Close();

            }
            catch (Exception e)
            {
                this.Page.Response.Write("<script language='JavaScript'>window.alert('" + e + "');</script>");
            }
        }

        protected void Exportar_click(object sender, EventArgs e)
        {
            string turno = Convert.ToString(drop_turno_d.SelectedValue);
            string linea_2 = Convert.ToString(drop_linea_d.SelectedValue);
            System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/sisconpt");
            System.Configuration.ConnectionStringSettings connStringmain;
            connStringmain = rootWebConfig.ConnectionStrings.ConnectionStrings["CONTROLPTConnectionString"];
            string PlantaNombre = Session["PlantaName"].ToString();
            SqlConnection con = new SqlConnection(connStringmain.ToString());

            int planta = Convert.ToInt32(txt_cod_plan.Text);
            string inicio = txt_fechainicio.Text;
            string fin = txt_fechafin.Text;

            string sql = "";

            if (linea_2 == "Todas")
            {
                sql = "[RESUMEN_CC_PAC_005_todas] '" + inicio + "','" + fin + "', '" + turno + "'," + planta + "; select * from ##a;";
            }
            else
            {
                sql = "[RESUMEN_CC_PAC_005] '" + inicio + "','" + fin + "', '" + turno + "'," + linea_2 + "," + planta + "; select * from ##a;";
            }

            SqlCommand command = new SqlCommand(sql, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            this.ExportToExcel(dt, "Resultado.xls");

        }

        protected void Filtrar(object sender, EventArgs e)
        {
            string turno = Convert.ToString(drop_turno_d.SelectedValue);
            string linea_2 = Convert.ToString(drop_linea_d.SelectedValue);
  
            string inicio = txt_fechainicio.Text;
            string fin = txt_fechafin.Text;

            GvProcesos_Llenar(turno, linea_2, inicio, fin);

        }

        public void ExportToExcel(DataTable dt, string filename)
        {
            if (dt.Rows.Count > 0)
            {
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                dgGrid.RenderControl(hw);
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
                Response.Redirect("~/SisConPT/Resumen_CC-PAC-005_CODCAJ.aspx");
            }
        }


    }
}