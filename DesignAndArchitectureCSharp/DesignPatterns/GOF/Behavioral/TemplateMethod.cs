using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Se crea una plantilla de metodos, La clase abstracta tendra funcionalidades bases y solo se extenderan aquellas funcionalidades
// que varian
namespace DesignAndArchitectureCSharp.DesignPatterns.GOF.Behavioral
{
    public abstract class DocumentProcessor
    {
        public void ProcessDocument()
        {
            InitializeDocument();
            LoadDocumentData();
            AnalyzeDocumentContent();
            SaveDocumentChanges();
            FinalizeDocument();
        }

        protected void InitializeDocument()
        {
            Console.WriteLine("DocumentProcessor dice: Inicializando documento...");
        }

        protected abstract void LoadDocumentData();

        protected void AnalyzeDocumentContent()
        {
            Console.WriteLine("DocumentProcessor dice: Analizando contenido del documento...");
        }

        protected abstract void SaveDocumentChanges();

        protected void FinalizeDocument()
        {
            Console.WriteLine("DocumentProcessor dice: Finalizando documento...");
        }

        protected virtual void PreProcessDocument() { }
        protected virtual void PostProcessDocument() { }
    }

    public class TextDocumentProcessor : DocumentProcessor
    {
        protected override void LoadDocumentData()
        {
            Console.WriteLine("Cargando datos de texto del documento...");
        }

        protected override void SaveDocumentChanges()
        {
            Console.WriteLine("Guardando cambios del documentos...");
        }

        protected override void PreProcessDocument()
        {
            Console.WriteLine("TextDocumentProcessor dice: Pre-procesando texto del documento...");
        }
    }
    public class ImageDocumentProcessor : DocumentProcessor
    {
        protected override void LoadDocumentData()
        {
            Console.WriteLine("Cargando datos de imagenes del documento...");
        }

        protected override void SaveDocumentChanges()
        {
            Console.WriteLine("Guarando cambios de imagenes del documento...");
        }

        protected override void PostProcessDocument()
        {
            Console.WriteLine("ImageDocumentProcessor dice: Post-procesando imagen del documento...");
        }
    }
}
