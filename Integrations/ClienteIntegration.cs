using System.Text;
using NDbfReader;

public class ClienteIntegration
{
    private readonly string _dbfPath = @"C:\CASH";
    private readonly string _tempPath = @"C:\ERP\TEMP";

    public List<ClientDto> BuscarClientes()
    {
        var clientes = new List<ClientDto>();

        // 📁 ORIGEM
        string origemDbf = Path.Combine(_dbfPath, "CLIENTES.DBF");
        string origemFpt = Path.Combine(_dbfPath, "CLIENTES.FPT");

        // 📁 DESTINO
        string destinoDbf = Path.Combine(_tempPath, "CLIENTES_COPY.DBF");
        string destinoFpt = Path.Combine(_tempPath, "CLIENTES_COPY.FPT");

        // Cria pasta TEMP
        if (!Directory.Exists(_tempPath))
            Directory.CreateDirectory(_tempPath);

        // Copia arquivos DBF + FPT
        CopiarArquivoSeguro(origemDbf, destinoDbf);
        if (File.Exists(origemFpt))
            CopiarArquivoSeguro(origemFpt, destinoFpt);

        Console.WriteLine("Arquivos copiados.");

        // 🔹 LEITURA COM NDbfReader (suporta MEMO)
        using (var table = Table.Open(destinoDbf))
        {
            var reader = table.OpenReader(Encoding.GetEncoding("ISO-8859-1"));

            while (reader.Read())
            {
                var cliente = new ClientDto();

                // Aqui estamos populando todos os campos do DTO
                cliente.COD_CLI = reader.GetValue("COD_CLI")?.ToString();
                cliente.NOME = reader.GetValue("NOME")?.ToString();
                cliente.FANTASIA = reader.GetValue("FANTASIA")?.ToString();
                cliente.DATANASC = reader.GetValue("DATANASC")?.ToString();
                cliente.ENDERECO = reader.GetValue("ENDERECO")?.ToString();
                cliente.XCPL = reader.GetValue("XCPL")?.ToString();
                cliente.BAIRRO = reader.GetValue("BAIRRO")?.ToString();
                cliente.CDBAIRRO = reader.GetValue("CDBAIRRO")?.ToString();
                cliente.CIDADE = reader.GetValue("CIDADE")?.ToString();
                cliente.COMPCIDADE = reader.GetValue("COMPCIDADE")?.ToString();
                cliente.CDCIDIBGE = reader.GetValue("CDCIDIBGE")?.ToString();
                cliente.ESTADO = reader.GetValue("ESTADO")?.ToString();
                cliente.CDUFIBGE = reader.GetValue("CDUFIBGE")?.ToString();
                cliente.CDPAIS = reader.GetValue("CDPAIS")?.ToString();
                cliente.PTOREF = reader.GetValue("PTOREF")?.ToString();
                cliente.ALIQ = reader.GetValue("ALIQ")?.ToString();
                cliente.COD_TRIB = reader.GetValue("COD_TRIB")?.ToString();
                cliente.ATIVAFF = reader.GetValue("ATIVAFF")?.ToString();
                cliente.CEP = reader.GetValue("CEP")?.ToString();
                cliente.CGC_CPF = reader.GetValue("CGC_CPF")?.ToString();
                cliente.INSC_RG = reader.GetValue("INSC_RG")?.ToString();
                cliente.INS_MUN = reader.GetValue("INS_MUN")?.ToString();
                cliente.ISUFRAMA = reader.GetValue("ISUFRAMA")?.ToString();
                cliente.CNAE = reader.GetValue("CNAE")?.ToString();
                cliente.LIM_CRED = reader.GetValue("LIM_CRED")?.ToString();
                cliente.LIM_MES = reader.GetValue("LIM_MES")?.ToString();
                cliente.FONE = reader.GetValue("FONE")?.ToString();
                cliente.FAX = reader.GetValue("FAX")?.ToString();
                cliente.E_MAIL = reader.GetValue("E_MAIL")?.ToString();
                cliente.HOMEPAGE = reader.GetValue("HOMEPAGE")?.ToString();
                cliente.MAE = reader.GetValue("MAE")?.ToString();
                cliente.PAI = reader.GetValue("PAI")?.ToString();
                cliente.TRABALHO = reader.GetValue("TRABALHO")?.ToString();
                cliente.ENDCOM = reader.GetValue("ENDCOM")?.ToString();
                cliente.FONECOM = reader.GetValue("FONECOM")?.ToString();
                cliente.COD_BLOQ = reader.GetValue("COD_BLOQ")?.ToString();
                cliente.DT_BLOQ = reader.GetValue("DT_BLOQ")?.ToString();
                cliente.OBS = reader.GetValue("OBS")?.ToString();
                cliente.MALA_DIR = reader.GetValue("MALA_DIR")?.ToString();
                cliente.EMIS_CART = reader.GetValue("EMIS_CART")?.ToString();
                cliente.DTVALCART = reader.GetValue("DTVALCART")?.ToString();
                cliente.DT_COMPRA = reader.GetValue("DT_COMPRA")?.ToString();
                cliente.DT_INSTAL = reader.GetValue("DT_INSTAL")?.ToString();
                cliente.DT_GARANT = reader.GetValue("DT_GARANT")?.ToString();
                cliente.COD_USU = reader.GetValue("COD_USU")?.ToString();
                cliente.TX_SERV = reader.GetValue("TX_SERV")?.ToString();
                cliente.DIA_PAG = reader.GetValue("DIA_PAG")?.ToString();
                cliente.TIPO_DOC = reader.GetValue("TIPO_DOC")?.ToString();
                cliente.COD_BAN = reader.GetValue("COD_BAN")?.ToString();
                cliente.COD_PRO = reader.GetValue("COD_PRO")?.ToString();
                cliente.LASTREAJ = reader.GetValue("LASTREAJ")?.ToString();
                cliente.COMISSAO = reader.GetValue("COMISSAO")?.ToString();
                cliente.VC = reader.GetValue("VC")?.ToString();
                cliente.QP = reader.GetValue("QP")?.ToString();
                cliente.CATEGORIA = reader.GetValue("CATEGORIA")?.ToString();
                cliente.DESCTO = reader.GetValue("DESCTO")?.ToString();
                cliente.DESC_REC = reader.GetValue("DESC_REC")?.ToString();
                cliente.PRAZO = reader.GetValue("PRAZO")?.ToString();
                cliente.DIA_INI1 = reader.GetValue("DIA_INI1")?.ToString();
                cliente.DIA_FIN1 = reader.GetValue("DIA_FIN1")?.ToString();
                cliente.DIA_INI2 = reader.GetValue("DIA_INI2")?.ToString();
                cliente.DIA_FIN2 = reader.GetValue("DIA_FIN2")?.ToString();
                cliente.DIAVENC = reader.GetValue("DIAVENC")?.ToString();
                cliente.THISMES = reader.GetValue("THISMES")?.ToString();
                cliente.DIAVENC2 = reader.GetValue("DIAVENC2")?.ToString();
                cliente.DOWNCOMI = reader.GetValue("DOWNCOMI")?.ToString();
                cliente.DATA_INCL = reader.GetValue("DATA_INCL")?.ToString();
                cliente.DATA_ALT = reader.GetValue("DATA_ALT")?.ToString();
                cliente.N_ALT = reader.GetValue("N_ALT")?.ToString();
                cliente.PROFISSAO = reader.GetValue("PROFISSAO")?.ToString();
                cliente.CONJUGE = reader.GetValue("CONJUGE")?.ToString();
                cliente.CONJCPF = reader.GetValue("CONJCPF")?.ToString();
                cliente.CONJRG = reader.GetValue("CONJRG")?.ToString();
                cliente.CONJNASC = reader.GetValue("CONJNASC")?.ToString();
                cliente.CONJPROF = reader.GetValue("CONJPROF")?.ToString();
                cliente.REFCOM1 = reader.GetValue("REFCOM1")?.ToString();
                cliente.REFCOM2 = reader.GetValue("REFCOM2")?.ToString();
                cliente.REFCOM3 = reader.GetValue("REFCOM3")?.ToString();
                cliente.REFBCO1 = reader.GetValue("REFBCO1")?.ToString();
                cliente.REFBCO2 = reader.GetValue("REFBCO2")?.ToString();
                cliente.REFBCO3 = reader.GetValue("REFBCO3")?.ToString();
                cliente.REFPES1 = reader.GetValue("REFPES1")?.ToString();
                cliente.REFPES2 = reader.GetValue("REFPES2")?.ToString();
                cliente.REFPES3 = reader.GetValue("REFPES3")?.ToString();
                cliente.RENDAPES = reader.GetValue("RENDAPES")?.ToString();
                cliente.RENDAFAM = reader.GetValue("RENDAFAM")?.ToString();
                cliente.ENTENDER = reader.GetValue("ENTENDER")?.ToString();
                cliente.ENTBAIRRO = reader.GetValue("ENTBAIRRO")?.ToString();
                cliente.ENTCIDADE = reader.GetValue("ENTCIDADE")?.ToString();
                cliente.ENTUF = reader.GetValue("ENTUF")?.ToString();
                cliente.ENTCEP = reader.GetValue("ENTCEP")?.ToString();
                cliente.ENTFONE = reader.GetValue("ENTFONE")?.ToString();
                cliente.COBENDER = reader.GetValue("COBENDER")?.ToString();
                cliente.COBBAIRRO = reader.GetValue("COBBAIRRO")?.ToString();
                cliente.COBCIDADE = reader.GetValue("COBCIDADE")?.ToString();
                cliente.COBUF = reader.GetValue("COBUF")?.ToString();
                cliente.COBCEP = reader.GetValue("COBCEP")?.ToString();
                cliente.COBFONE = reader.GetValue("COBFONE")?.ToString();
                cliente.RESTIPO = reader.GetValue("RESTIPO")?.ToString();
                cliente.RESALUG = reader.GetValue("RESALUG")?.ToString();
                cliente.RESTEMPO = reader.GetValue("RESTEMPO")?.ToString();
                cliente.ADMISSAO = reader.GetValue("ADMISSAO")?.ToString();
                cliente.CPTS = reader.GetValue("CPTS")?.ToString();
                cliente.MATRICULA = reader.GetValue("MATRICULA")?.ToString();
                cliente.PAICPF = reader.GetValue("PAICPF")?.ToString();
                cliente.PAIRG = reader.GetValue("PAIRG")?.ToString();
                cliente.PAINASC = reader.GetValue("PAINASC")?.ToString();
                cliente.MAECPF = reader.GetValue("MAECPF")?.ToString();
                cliente.MAERG = reader.GetValue("MAERG")?.ToString();
                cliente.MAENASC = reader.GetValue("MAENASC")?.ToString();
                cliente.CONTATO1 = reader.GetValue("CONTATO1")?.ToString();
                cliente.CONTATO2 = reader.GetValue("CONTATO2")?.ToString();
                cliente.BANCO = reader.GetValue("BANCO")?.ToString();
                cliente.N_AGENCIA = reader.GetValue("N_AGENCIA")?.ToString();
                cliente.N_CONTA = reader.GetValue("N_CONTA")?.ToString();
                cliente.COD_TBPR = reader.GetValue("COD_TBPR")?.ToString();
                cliente.COD_CONV = reader.GetValue("COD_CONV")?.ToString();
                cliente.ISEPP = reader.GetValue("ISEPP")?.ToString();
                cliente.ISCONSUMO = reader.GetValue("ISCONSUMO")?.ToString();
                cliente.ISORGPUB = reader.GetValue("ISORGPUB")?.ToString();
                cliente.ISINDUST = reader.GetValue("ISINDUST")?.ToString();
                cliente.NFEMODDANF = reader.GetValue("NFEMODDANF")?.ToString();
                cliente.STCOFINS = reader.GetValue("STCOFINS")?.ToString();
                cliente.INATIVO = reader.GetValue("INATIVO")?.ToString();
                cliente.APOSENTADO = reader.GetValue("APOSENTADO")?.ToString();
                cliente.PESSOA = reader.GetValue("PESSOA")?.ToString();
                cliente.ESTCIVIL = reader.GetValue("ESTCIVIL")?.ToString();
                cliente.TXFACTOR = reader.GetValue("TXFACTOR")?.ToString();
                cliente.TIPOINDIC = reader.GetValue("TIPOINDIC")?.ToString();
                cliente.INDICADOR = reader.GetValue("INDICADOR")?.ToString();
                cliente.ALIQISS = reader.GetValue("ALIQISS")?.ToString();
                cliente.ISSR = reader.GetValue("ISSR")?.ToString();
                cliente.DESCFIN = reader.GetValue("DESCFIN")?.ToString();
                cliente.NATURAL = reader.GetValue("NATURAL")?.ToString();
                cliente.COD_COND = reader.GetValue("COD_COND")?.ToString();
                cliente.COD_TRN = reader.GetValue("COD_TRN")?.ToString();
                cliente.ACRE_VEND = reader.GetValue("ACRE_VEND")?.ToString();
                cliente.DTCONTATO = reader.GetValue("DTCONTATO")?.ToString();
                cliente.CODCONV = reader.GetValue("CODCONV")?.ToString();
                cliente.VR_FRETE = reader.GetValue("VR_FRETE")?.ToString();
                cliente.CONTRIBU = reader.GetValue("CONTRIBU")?.ToString();
                cliente.PORTCATPRO = reader.GetValue("PORTCATPRO")?.ToString();
                cliente.NFEOBS = reader.GetValue("NFEOBS")?.ToString();
                cliente.DIASPROT = reader.GetValue("DIASPROT")?.ToString();
                cliente.PROTESTO = reader.GetValue("PROTESTO")?.ToString();
                cliente.CONSELHO = reader.GetValue("CONSELHO")?.ToString();
                cliente.CONTADEB = reader.GetValue("CONTADEB")?.ToString();
                cliente.CONTACRE = reader.GetValue("CONTACRE")?.ToString();

                clientes.Add(cliente);
            }
        }

        return clientes;
    }

    // 🔒 CÓPIA SEGURA (evita lock do Delphi)
    private void CopiarArquivoSeguro(string origem, string destino)
    {
        using (FileStream source = new FileStream(origem, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (FileStream dest = new FileStream(destino, FileMode.Create, FileAccess.Write))
        {
            source.CopyTo(dest);
        }
    }
}