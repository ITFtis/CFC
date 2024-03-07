using CFC.Models.Prj;
using System.Collections.Generic;

namespace CFC.Models.Api
{
    public class CalInputModel
    {

        public string UserID { get; set; }

        public int RowID { get; set; } // ���ϥΪ̫إ߱M�׹�Ϊ�

        public string calModel { get; set; }

        // �M�׫إ߻ݨD
        public bool IsSave { get; set; }// �O�_�x�s���M��
        public string ProjectName { get; set; }//�M�צW��
        public string ProjectIndustrialID { get; set; }//�M�׳��n
        public string ProjectAddress { get; set; }//�M�צa�}
        public string ProjectCity { get; set; }//�M�׿���
        public string ProjectIndustrialType { get; set; }//�M�צ�~�O

        // ��L��J��

        //�U��
        public List<FuelInputs> fuelInputs { get; set; }

        //�q�O
        public ElectInput electInput { get; set; }

        // �N�C
        public List<RefrigerantInput> refrigerantInputs { get; set; }

        //�h��
        public List<EscapeInput> escapeInputs { get; set; }

        // �]��
        public SteamInput steamInput { get; set; }

        // �S��s�{
        public List<SpecialInput> specialInputs { get; set; }



        // ���O3
        public double Tr01 { get; set; } // ���O3-�B��-�W��쪫�ưt�e��q
        public double Tr02 { get; set; } // ���O3 - �B�� - �ӰȮȹC
        public double Tr03 { get; set; } // ���O3 - �B�� - ���u�q��
        public double Tr04 { get; set; } // ���O3 - �B�� - �U��B��ΰt�e
        public double Cp01 { get; set; } // ���O3 - ��´�ϥβ��~ - ����
        public double Cp02 { get; set; } // ���O3 - ��´�ϥβ��~ - �ꥻ
        public double Cp03 { get; set; } // ���O3 - ��´�ϥβ��~ - �෽��������
        public double Cp04 { get; set; } // ���O3 - ��´�ϥβ��~ - ��B�o��
        public double Cp05 { get; set; } // ���O3 - ��´�ϥβ��~ - �W��겣����
        public double Us01 { get; set; } // ���O3 - �ϥβ�´���~ - �[�u
        public double Us02 { get; set; } // ���O3 - �ϥβ�´���~ - �ϥ�
        public double Us03 { get; set; } // ���O3 - �ϥβ�´���~ - ���o
        public double Us04 { get; set; } // ���O3 - �ϥβ�´���~ - �U�寲��
        public double Us05 { get; set; } // ���O3 - �ϥβ�´���~ - �[��
        public double Us06 { get; set; } // ���O3 - �ϥβ�´���~ - ���
        public double Other { get; set; } // ���O3 - ��L�Ʃ�

    }
}