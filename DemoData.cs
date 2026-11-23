using System.Data;
using DoRuan.Report.Model;

namespace DoRuan.Report.Demo;

public static class DemoData
{
    public static ReportData CreatePurchaseOrder()
    {
        var header = new DataTable("单据头");
        header.Columns.Add("订单编号", typeof(string));
        header.Columns.Add("订单日期", typeof(DateTime));
        header.Columns.Add("供应商", typeof(string));
        header.Columns.Add("采购员", typeof(string));
        header.Columns.Add("仓库", typeof(string));
        header.Columns.Add("备注", typeof(string));

        header.Rows.Add(
            "CG20261117001",
            new DateTime(2026, 11, 17),
            "苏州精工五金有限公司",
            "张伟",
            "原料仓",
            "请于 2026-11-25 前到货，验收合格后 30 天内付款。");

        var details = new DataTable("明细数据");
        details.Columns.Add("序号", typeof(int));
        details.Columns.Add("物料编码", typeof(string));
        details.Columns.Add("物料名称", typeof(string));
        details.Columns.Add("规格型号", typeof(string));
        details.Columns.Add("单位", typeof(string));
        details.Columns.Add("数量", typeof(decimal));
        details.Columns.Add("单价", typeof(decimal));
        details.Columns.Add("金额", typeof(decimal));
        details.Columns.Add("交期", typeof(DateTime));

        AddRow(1, "WL-1001", "内六角螺丝", "M6×20", "个", 500, 0.12m, new DateTime(2026, 11, 20));
        AddRow(2, "WL-1002", "六角螺母", "M6", "个", 500, 0.08m, new DateTime(2026, 11, 20));
        AddRow(3, "WL-1003", "平垫片", "M6", "个", 500, 0.05m, new DateTime(2026, 11, 20));
        AddRow(4, "WL-2001", "工业铝型材", "4040×3000", "根", 20, 68.50m, new DateTime(2026, 11, 22));
        AddRow(5, "WL-2002", "铝型材角码", "4040", "个", 80, 2.30m, new DateTime(2026, 11, 22));
        AddRow(6, "WL-3001", "步进电机", "57BYG250B", "台", 4, 268.00m, new DateTime(2026, 11, 24));
        AddRow(7, "WL-3002", "步进驱动器", "DM542", "台", 4, 195.00m, new DateTime(2026, 11, 24));
        AddRow(8, "WL-4001", "护套电缆", "RVV 4×1.5", "米", 200, 6.80m, new DateTime(2026, 11, 25));

        return new ReportData()
            .AddTable("单据头", header)
            .AddTable("明细数据", details);

        void AddRow(int index, string code, string name, string spec, string unit, decimal quantity, decimal price, DateTime delivery)
        {
            details.Rows.Add(index, code, name, spec, unit, quantity, price, quantity * price, delivery);
        }
    }
}
