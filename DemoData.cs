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
        details.Columns.Add("分类", typeof(string));
        details.Columns.Add("物料编码", typeof(string));
        details.Columns.Add("物料名称", typeof(string));
        details.Columns.Add("规格型号", typeof(string));
        details.Columns.Add("单位", typeof(string));
        details.Columns.Add("数量", typeof(decimal));
        details.Columns.Add("单价", typeof(decimal));
        details.Columns.Add("金额", typeof(decimal));
        details.Columns.Add("交期", typeof(DateTime));

        AddRow(1, "紧固件", "WL-1001", "内六角螺丝", "M6×20", "个", 500, 0.12m, new DateTime(2026, 11, 20));
        AddRow(2, "紧固件", "WL-1002", "六角螺母", "M6", "个", 500, 0.08m, new DateTime(2026, 11, 20));
        AddRow(3, "紧固件", "WL-1003", "平垫片", "M6", "个", 500, 0.05m, new DateTime(2026, 11, 20));
        AddRow(4, "紧固件", "WL-1004", "弹簧垫圈", "M6", "个", 500, 0.04m, new DateTime(2026, 11, 20));
        AddRow(5, "型材", "WL-2001", "工业铝型材", "4040×3000", "根", 20, 68.50m, new DateTime(2026, 11, 22));
        AddRow(6, "型材", "WL-2002", "铝型材角码", "4040", "个", 80, 2.30m, new DateTime(2026, 11, 22));
        AddRow(7, "型材", "WL-2003", "铝型材端盖", "4040", "个", 80, 0.65m, new DateTime(2026, 11, 22));
        AddRow(8, "电气", "WL-3001", "步进电机", "57BYG250B", "台", 4, 268.00m, new DateTime(2026, 11, 24));
        AddRow(9, "电气", "WL-3002", "步进驱动器", "DM542", "台", 4, 195.00m, new DateTime(2026, 11, 24));
        AddRow(10, "电气", "WL-3003", "开关电源", "S-350-24", "台", 2, 118.00m, new DateTime(2026, 11, 24));
        AddRow(11, "线缆", "WL-4001", "护套电缆", "RVV 4×1.5", "米", 200, 6.80m, new DateTime(2026, 11, 25));
        AddRow(12, "线缆", "WL-4002", "屏蔽电缆", "RVVP 2×0.5", "米", 100, 3.20m, new DateTime(2026, 11, 25));

        // 子报表测试数据一：按订单编号关联，适合放在汇总/页脚里整段渲染一次。
        var payments = new DataTable("付款记录");
        payments.Columns.Add("订单编号", typeof(string));
        payments.Columns.Add("付款日期", typeof(DateTime));
        payments.Columns.Add("付款方式", typeof(string));
        payments.Columns.Add("金额", typeof(decimal));
        payments.Columns.Add("经办人", typeof(string));
        payments.Rows.Add("CG20261117001", new DateTime(2026, 11, 18), "银行转账", 1655.70m, "财务部 李娜");
        payments.Rows.Add("CG20261117001", new DateTime(2026, 11, 26), "银行转账", 3863.30m, "财务部 李娜");

        // 子报表测试数据二：按物料编码关联，适合放进明细带做“每行一份”主从子报表。
        var batches = new DataTable("入库批次");
        batches.Columns.Add("物料编码", typeof(string));
        batches.Columns.Add("批次号", typeof(string));
        batches.Columns.Add("数量", typeof(decimal));
        batches.Columns.Add("入库日期", typeof(DateTime));
        batches.Columns.Add("库位", typeof(string));
        batches.Rows.Add("WL-1001", "B2601", 200, new DateTime(2026, 11, 20), "A-01-02");
        batches.Rows.Add("WL-1001", "B2602", 300, new DateTime(2026, 11, 21), "A-01-03");
        batches.Rows.Add("WL-2001", "B2603", 10, new DateTime(2026, 11, 22), "B-02-01");
        batches.Rows.Add("WL-2001", "B2604", 10, new DateTime(2026, 11, 24), "B-02-01");
        batches.Rows.Add("WL-3001", "B2605", 4, new DateTime(2026, 11, 24), "C-03-01");

        return new ReportData()
            .AddTable("单据头", header)
            .AddTable("明细数据", details)
            .AddTable("付款记录", payments)
            .AddTable("入库批次", batches);

        void AddRow(int index, string category, string code, string name, string spec, string unit,
            decimal quantity, decimal price, DateTime delivery)
        {
            details.Rows.Add(index, category, code, name, spec, unit, quantity, price, quantity * price, delivery);
        }
    }
}
