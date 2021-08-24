using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace cs_form_framework_mail_mailkit
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void sendButton_Click(object sender, EventArgs e)
		{
			var enc_jis = System.Text.Encoding.GetEncoding("iso-2022-jp");
			var host = server.Text;
			var port = portNo.Text;

			using (var smtp = new MailKit.Net.Smtp.SmtpClient())
			{

                try
                {
                    //SMTPサーバに接続する( SSL で接続 )
                    smtp.Connect(host, Int32.Parse(port), MailKit.Security.SecureSocketOptions.SslOnConnect);
                    //認証が必要な場合は、以下のコメントを解除
                    smtp.Authenticate(account.Text, password.Text);

                    //送信するメールを作成する
                    var mail = new MimeKit.MimeMessage();
                    var builder = new MimeKit.BodyBuilder();

                    mail.From.Add(new MimeKit.MailboxAddress("わたし", fromAddress.Text));
                    mail.To.Add(new MimeKit.MailboxAddress("あなた", toAddress.Text));

                    // 件名
                    mail.Headers.Replace(MimeKit.HeaderId.Subject, enc_jis, subject.Text);

                    // JIS で本文を作成する
                    MimeKit.TextPart textPart = new MimeKit.TextPart("plain");
                    textPart.SetText(enc_jis, textBody.Text);
                    // "iso-2022-jp"で送るので、"Content-Transfer-Encoding"に"7bit"を指定
                    textPart.ContentTransferEncoding = MimeKit.ContentEncoding.SevenBit;

                    // MimeMessage に本文をセット
                    mail.Body = textPart;

                    //メールを送信する
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    MessageBox.Show("メール送信に失敗しました");
                }

				//SMTPサーバから切断する
				smtp.Disconnect(true);
			}
		}
	}
}