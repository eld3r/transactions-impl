using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transactions.Dal.PostgresEfCore.Migrations
{
    public partial class TransactionLimitTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION limit_transaction_count()
                RETURNS trigger AS $$
                BEGIN
                    IF (SELECT COUNT(*) FROM ""transactions"") >= 100 THEN
                        RAISE EXCEPTION 'Cannot insert more than 100 transactions' USING ERRCODE = 'P0003';
                    END IF;
                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_limit_transactions
                BEFORE INSERT ON ""transactions""
                FOR EACH ROW
                EXECUTE FUNCTION limit_transaction_count();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS trg_limit_transactions ON ""transactions"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS limit_transaction_count();");
        }
    }
}
