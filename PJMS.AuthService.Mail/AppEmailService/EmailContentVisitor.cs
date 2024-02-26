using Microsoft.Extensions.Localization;
using PJMS.AuthService.Abstractions.AppEmailService;
using PJMS.AuthService.Abstractions.AppEmailService.Structs;
using PJMS.AuthService.Mail.AppEmailService.Structs;

namespace PJMS.AuthService.Mail.AppEmailService;

/// <inheritdoc/>
/// <summary>
/// Интерфейс посетителя Email
/// </summary>
/// <param name="localizer">Локализатор</param>
/// <param name="configuration">Настройки шаблона письма</param>
public class EmailContentVisitor(EmailTemplateConfiguration configuration, IStringLocalizer<EmailService> localizer)
    : IEmailVisitor
{
    /// <summary>
    /// Получает или задает HTML-контент электронной почты.
    /// </summary>
    public string? HtmlContent { get; private set; }

    /// <summary>
    /// Получает или задает тему электронной почты.
    /// </summary>
    public string? Subject { get; private set; }

    /// <inheritdoc/>
    /// <summary>
    /// Посещает ConfirmRegistrationEmail.
    /// </summary>
    public void Visit(ConfirmRegistrationEmail email)
    {
        // Задаем значение переменной Subject из локализатора, используя ключ "ConfirmRegistrationEmailSubject"
        Subject = localizer["ConfirmRegistrationEmailSubject"];

        // Создаем переменную text и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailText"
        var text = localizer["ConfirmRegistrationEmailText"];

        // Создаем переменную explanation и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailExplanation"
        var explanation = localizer["ConfirmRegistrationEmailExplanation"];

        // Создаем переменную buttonName и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailButtonName"
        var buttonName = localizer["ConfirmRegistrationEmailButtonName"];

        // Создаем переменную expiration и устанавливаем значение из локализатора "LinkExpires"
        var expiration = localizer["LinkExpires"];

        // Вызываем метод EmailTemplate и присваиваем результат переменной HtmlContent
        HtmlContent = EmailTemplate(Subject, explanation, text, ButtonTemplate(buttonName, email.ConfirmLink),
            expiration, email.Recipient);
    }

    /// <inheritdoc/>
    /// <summary>
    /// Посещает ConfirmRecoverPasswordEmail.
    /// </summary>
    public void Visit(ConfirmRecoverPasswordEmail email)
    {
        // Задаем значение переменной Subject из локализатора, используя ключ "ConfirmRegistrationEmailSubject"
        Subject = localizer["ConfirmRecoverPasswordEmailSubject"];

        // Создаем переменную text и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailText"
        var text = localizer["ConfirmRecoverPasswordEmailText"];

        // Создаем переменную explanation и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailExplanation"
        var explanation = localizer["ConfirmRecoverPasswordEmailExplanation"];

        // Создаем переменную buttonName и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailButtonName"
        var buttonName = localizer["ConfirmRecoverPasswordEmailButtonName"];

        // Создаем переменную expiration и устанавливаем значение из локализатора "LinkExpires"
        var expiration = localizer["LinkExpires"];

        // Вызываем метод EmailTemplate и присваиваем результат переменной HtmlContent
        HtmlContent = EmailTemplate(Subject, explanation, text, ButtonTemplate(buttonName, email.ConfirmLink),
            expiration,
            email.Recipient);
    }

    /// <inheritdoc/>
    /// <summary>
    /// Посещает ConfirmMailChangeEmail.
    /// </summary>
    public void Visit(ConfirmMailChangeEmail email)
    {
        // Задаем значение переменной Subject из локализатора, используя ключ "ConfirmRegistrationEmailSubject"
        Subject = localizer["ConfirmMailChangeEmailSubject"];

        // Создаем переменную text и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailText"
        var text = localizer["ConfirmMailChangeEmailText"];

        // Создаем переменную explanation и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailExplanation"
        var explanation = localizer["ConfirmMailChangeEmailExplanation"];

        // Создаем переменную buttonName и задаем ей значение из локализатора, используя ключ "ConfirmRegistrationEmailButtonName"
        var buttonName = localizer["ConfirmMailChangeEmailButtonName"];

        // Создаем переменную expiration и устанавливаем значение из локализатора "LinkExpires"
        var expiration = localizer["LinkExpires"];

        // Вызываем метод EmailTemplate и присваиваем результат переменной HtmlContent
        HtmlContent = EmailTemplate(Subject, explanation, text, ButtonTemplate(buttonName, email.ConfirmLink),
            expiration,
            email.Recipient);
    }

    /// <summary>
    /// Посещает TwoFactorCodeEmail
    /// </summary>
    public void Visit(TwoFactorCodeEmail email)
    {
        // Задаем значение переменной Subject из локализатора, используя ключ "TwoFactorCodeEmailSubject"
        Subject = localizer["TwoFactorCodeEmailSubject"];

        // Создаем переменную text и задаем ей значение из локализатора, используя ключ "TwoFactorCodeEmailText"
        var text = localizer["TwoFactorCodeEmailText"];

        //// Создаем переменную explanation и задаем ей значение из локализатора, используя ключ "TwoFactorCodeEmailExplanation"
        var explanation = localizer["TwoFactorCodeEmailExplanation"];

        // Создаем переменную expiration и устанавливаем значение из локализатора "LinkExpires"
        var expiration = localizer["CodeExpires"];

        // Вызываем метод EmailTemplate и присваиваем результат переменной HtmlContent
        HtmlContent = EmailTemplate(Subject, explanation, text, CodeTemplate(email.Code), expiration, email.Recipient);
    }

    /// <summary>
    /// Шаблон кнопки в письме
    /// </summary>
    /// <param name="buttonName">Название кнопки</param>
    /// <param name="buttonLink">Ссылка, которая откроется по нажатию кнопки</param>
    /// <returns>HTML разметка с кнопкой</returns>
    private static string ButtonTemplate(string buttonName, string buttonLink)
    {
        // Отдаем контент письма
        return $"""
                <table cellpadding="0" cellspacing="0" width="100%"
                       role="presentation"
                       style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                    <tr>
                        <td align="center" style="padding:0;Margin:0">
                            <span
                                    class="msohide es-button-border"
                                    style="border-style:solid;border-color:#2CB543;background:#7630f3;border-width:0;display:block;border-radius:30px;width:auto;mso-hide:all"><a
                                    href="{buttonLink}" class="es-button msohide"
                                    target="_blank"
                                    style="mso-style-priority:100 !important;text-decoration:none;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;color:#FFFFFF;font-size:22px;display:block;background:#7630f3;border-radius:30px;font-family:Arial, sans-serif;font-weight:bold;font-style:normal;line-height:26px;width:auto;text-align:center;mso-padding-alt:0;mso-border-alt:10px solid  #7630f3;mso-hide:all;padding: 15px 5px;">{buttonName}</a></span>
                        </td>
                    </tr>
                </table>
                """;
    }

    /// <summary>
    /// Шаблон кода в письме
    /// </summary>
    /// <param name="code">Код</param>
    /// <returns>HTML разметка с кодом</returns>
    private static string CodeTemplate(string code)
    {
        // Отдаем контент письма
        return $"""
                <div style="background-color: rgb(51, 51, 51); color: rgb(241, 241, 241); border-radius: 5px; letter-spacing: 2px; padding: 1px;">
                      <h4>{code}</h4>
                </div>
                """;
    }

    /// <summary>
    /// Шаблон  письма с кнопкой (без отписки)
    /// </summary>
    /// <param name="title">Название письма</param>
    /// <param name="explanation">Пояснение, почему письмо было отправлено</param>
    /// <param name="text">Текст письма</param>
    /// <param name="content">Контент письма</param>
    /// <param name="expiration">Текст блока о сроке действия</param>
    /// <param name="recipient">Получатель письма</param>
    /// <returns>HTML письма</returns>
    private string EmailTemplate(string title, string explanation, string text, string content, string expiration,
        string recipient)
    {
        // Отдаем контент письма
        return
            $$"""
              <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
                      "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
              <html dir="ltr" xmlns="http://www.w3.org/1999/xhtml" lang="ru" style="font-family:arial, sans-serif">
              <head>
                  <title>{{title}}</title>
                  <style type="text/css">
                      a {
                          text-decoration: none;
                      }
              
                      sup {
                          font-size: 100% !important;
                      }
              
                      #outlook a {
                          padding: 0;
                      }
              
                      .es-button {
                          text-decoration: none !important;
                      }
              
                      a[x-apple-data-detectors] {
                          color: inherit !important;
                          text-decoration: none !important;
                          font-size: inherit !important;
                          font-family: inherit !important;
                          font-weight: inherit !important;
                          line-height: inherit !important;
                      }
              
                      @media only screen and (max-width: 600px) {
                          p, ul li, ol li, a {
                              line-height: 150% !important
                          }
              
                          h1, h2, h3, h1 a, h2 a, h3 a {
                              line-height: 120%
                          }
              
                          h1 {
                              font-size: 30px !important;
                              text-align: left
                          }
              
                          h2 {
                              font-size: 24px !important;
                              text-align: left
                          }
              
                          h3 {
                              font-size: 20px !important;
                              text-align: left
                          }
              
                          .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a {
                              font-size: 30px !important;
                              text-align: left
                          }
              
                          .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a {
                              font-size: 24px !important;
                              text-align: left
                          }
              
                          .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a {
                              font-size: 20px !important;
                              text-align: left
                          }
              
                          .es-menu td a {
                              font-size: 14px !important
                          }
              
                          .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a {
                              font-size: 14px !important
                          }
              
                          .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a {
                              font-size: 14px !important
                          }
              
                          .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a {
                              font-size: 14px !important
                          }
              
                          .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a {
                              font-size: 12px !important
                          }
              
                          *[class="gmail-fix"] {
                              display: none !important
                          }
              
                          .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 {
                              text-align: center !important
                          }
              
                          .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 {
                              text-align: left !important
                          }
              
                          .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img {
                              display: inline !important
                          }
              
                          .es-button-border {
                              display: block !important
                          }
              
                          a.es-button, button.es-button {
                              font-size: 18px !important;
                              display: block !important;
                              border-right-width: 0px !important;
                              border-left-width: 0px !important;
                              border-top-width: 15px !important;
                              border-bottom-width: 15px !important
                          }
              
                          .es-adaptive table, .es-left, .es-right {
                              width: 100% !important
                          }
              
                          .es-content table, .es-header table, .es-footer table, .es-content, .es-footer {
                              width: 100% !important;
                              max-width: 600px !important
                          }
              
                          .es-menu td {
                              width: 1% !important
                          }
              
                          table.es-social td {
                              display: inline-block !important
                          }
                      }
                  </style>
              </head>
              <body style="width:100%;font-family:arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0">
              <div dir="ltr" class="es-wrapper-color" lang="ru" style="background-color:#FFFFFF">
                  <table class="es-wrapper" width="100%" cellspacing="0" cellpadding="0" role="none"
                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;background-color:#FFFFFF">
                      <tr>
                          <td valign="top" style="padding:0;Margin:0">
                              <table cellpadding="0" cellspacing="0" class="es-footer" align="center" role="none"
                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top">
                                  <tr>
                                      <td align="center" style="padding:0;Margin:0">
                                          <table bgcolor="#bcb8b1" class="es-footer-body" align="center" cellpadding="0"
                                                 cellspacing="0" role="none"
                                                 style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;background-color:#FFFFFF;width:600px">
                                              <tr>
                                                  <td align="left"
                                                      style="Margin:0;padding: 20px 40px;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="center" valign="top" style="padding:0;Margin:0;width:520px">
                                                                  <table cellpadding="0" cellspacing="0" width="100%"
                                                                         role="presentation"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                      <tr>
                                                                          <td align="center" style="padding:0;Margin:0;font-size:0">
                                                                              <a target="_blank" href="{{configuration.HomePageLink}}"
                                                                                 style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#2D3142;font-size:14px"><img
                                                                                      src="{{configuration.SideLogoLink}}"
                                                                                      alt="Logo"
                                                                                      style="display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic"
                                                                                      height="60" title="Logo"></a></td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                              <table cellpadding="0" cellspacing="0" class="es-content" align="center" role="none"
                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;table-layout:fixed !important;width:100%">
                                  <tr>
                                      <td align="center" style="padding:0;Margin:0">
                                          <table bgcolor="#efefef" class="es-content-body" align="center" cellpadding="0"
                                                 cellspacing="0"
                                                 style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;background-color:#EFEFEF;border-radius:20px 20px 0 0;width:600px"
                                                 role="none">
                                              <tr>
                                                  <td align="left"
                                                      style="Margin:0;padding: 40px 40px 0;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="center" valign="top" style="padding:0;Margin:0;width:520px">
                                                                  <table cellpadding="0" cellspacing="0" width="100%"
                                                                         role="presentation"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                      <tr>
                                                                          <td align="left" class="es-m-txt-c"
                                                                              style="padding:0;Margin:0;font-size:0"><a
                                                                                  target="_blank" href="{{configuration.HomePageLink}}"
                                                                                  style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#2D3142;font-size:18px"><img
                                                                                  src="{{configuration.LogoLink}}"
                                                                                  alt="{{configuration.CompanyName}}"
                                                                                  style="display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;border-radius:100px;"
                                                                                  width="100" title="{{configuration.CompanyName}}"></a></td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td align="left"
                                                      style="Margin:0;padding: 20px 40px 0;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="center" valign="top" style="padding:0;Margin:0;width:520px">
                                                                  <table cellpadding="0" cellspacing="0" width="100%"
                                                                         bgcolor="#fafafa"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:separate;border-spacing:0;background-color:#fafafa;border-radius:10px"
                                                                         role="presentation">
                                                                      <tr>
                                                                          <td align="left" style="padding:20px;Margin:0"><h3
                                                                                  style="Margin:0;line-height:34px;mso-line-height-rule:exactly;font-family:Arial, sans-serif;font-size:20px;font-style:normal;font-weight:bold;color:#2D3142">
                                                                              {{localizer["Welcome"]}},&nbsp;{{recipient}}</h3>
                                                                              <p style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:27px;color:#2D3142;font-size:18px">
                                                                                  <br></p>
                                                                              <p style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:27px;color:#2D3142;font-size:18px">
                                                                                  {{explanation}}
                                                                                  <br><br>
                                                                                  {{text}}
                                                                                  </p>
                                                                            </td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                              <table cellpadding="0" cellspacing="0" class="es-content" align="center" role="none"
                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;table-layout:fixed !important;width:100%">
                                  <tr>
                                      <td align="center" style="padding:0;Margin:0">
                                          <table bgcolor="#efefef" class="es-content-body" align="center" cellpadding="0"
                                                 cellspacing="0" role="none"
                                                 style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;background-color:#EFEFEF;width:600px">
                                              <tr>
                                                  <td align="left"
                                                      style="Margin:0;padding: 30px 40px 40px;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="center" valign="top" style="padding:0;Margin:0;width:520px">
                                                                  {{content}}
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                              <tr>
                                                  <td align="left" style="Margin:0;padding: 0 40px;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="center" valign="top" style="padding:0;Margin:0;width:520px">
                                                                  <table cellpadding="0" cellspacing="0" width="100%"
                                                                         role="presentation"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                      <tr>
                                                                          <td align="left" style="padding:0;Margin:0"><p
                                                                                  style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:27px;color:#2D3142;font-size:18px">
                                                                              {{localizer["Thanks"]}},<br><br>{{configuration.CompanyName}}</p></td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td align="center"
                                                                              style="Margin:0;padding: 40px 0 20px;font-size:0">
                                                                              <table border="0" width="100%" height="100%"
                                                                                     cellpadding="0" cellspacing="0"
                                                                                     role="presentation"
                                                                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                                  <tr>
                                                                                      <td style="padding:0;Margin:0;border-bottom:1px solid #666666;background:unset;height:1px;width:100%;margin:0"></td>
                                                                                  </tr>
                                                                              </table>
                                                                          </td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                              <table cellpadding="0" cellspacing="0" class="es-content" align="center" role="none"
                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;table-layout:fixed !important;width:100%">
                                  <tr>
                                      <td align="center" style="padding:0;Margin:0">
                                          <table bgcolor="#efefef" class="es-content-body" align="center" cellpadding="0"
                                                 cellspacing="0"
                                                 style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;background-color:#EFEFEF;border-radius:0 0 20px 20px;width:600px"
                                                 role="none">
                                              <tr>
                                                  <td class="esdev-adapt-off" align="left"
                                                      style="Margin:0;padding: 20px 40px;">
                                                      <table cellpadding="0" cellspacing="0" class="esdev-mso-table" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;width:520px">
                                                          <tr>
                                                              <td class="esdev-mso-td" valign="top" style="padding:0;Margin:0">
                                                                  <table cellpadding="0" cellspacing="0" align="left" class="es-left"
                                                                         role="none"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;float:left">
                                                                      <tr>
                                                                          <td align="center" valign="top"
                                                                              style="padding:0;Margin:0;width:47px">
                                                                              <table cellpadding="0" cellspacing="0" width="100%"
                                                                                     role="presentation"
                                                                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                                  <tr>
                                                                                      <td align="center" class="es-m-txt-l"
                                                                                          style="padding:0;Margin:0;font-size:0"><a
                                                                                              target="_blank"
                                                                                              href="{{configuration.HomePageLink}}"
                                                                                              style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#2D3142;font-size:18px"><img
                                                                                              src="{{configuration.LogoLink}}"
                                                                                              alt="{{configuration.CompanyName}}"
                                                                                              style="border-radius:100px;display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic"
                                                                                              width="47" title="Demo"></a></td>
                                                                                  </tr>
                                                                              </table>
                                                                          </td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                              <td style="padding:0;Margin:0;width:20px"></td>
                                                              <td class="esdev-mso-td" valign="top" style="padding:0;Margin:0">
                                                                  <table cellpadding="0" cellspacing="0" class="es-right"
                                                                         align="right" role="none"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;float:right">
                                                                      <tr>
                                                                          <td align="center" valign="top"
                                                                              style="padding:0;Margin:0;width:453px">
                                                                              <table cellpadding="0" cellspacing="0" width="100%"
                                                                                     role="presentation"
                                                                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                                  <tr>
                                                                                      <td align="left" style="padding:0;Margin:0">
                                                                                          <p style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:24px;color:#2D3142;font-size:16px">
                                                                                              {{expiration}}
                                                                                          </p>
                                                                                      </td>
                                                                                  </tr>
                                                                              </table>
                                                                          </td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                              <table cellpadding="0" cellspacing="0" class="es-footer" align="center" role="none"
                                     style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top">
                                  <tr>
                                      <td align="center" style="padding:0;Margin:0">
                                          <table bgcolor="#bcb8b1" class="es-footer-body" align="center" cellpadding="0"
                                                 cellspacing="0" role="none"
                                                 style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0;background-color:#FFFFFF;width:600px">
                                              <tr>
                                                  <td align="left"
                                                      style="Margin:0;padding: 40px 20px 30px;">
                                                      <table cellpadding="0" cellspacing="0" width="100%" role="none"
                                                             style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                          <tr>
                                                              <td align="left" style="padding:0;Margin:0;width:560px">
                                                                  <table cellpadding="0" cellspacing="0" width="100%"
                                                                         role="presentation"
                                                                         style="mso-table-lspace:0;mso-table-rspace:0;border-collapse:collapse;border-spacing:0">
                                                                      <tr>
                                                                          <td align="center" class="es-m-txt-c"
                                                                              style="Margin:0;padding: 0 0 20px;font-size:0">
                                                                              <img src="{{configuration.SideLogoLink}}"
                                                                                   alt="Logo"
                                                                                   style="display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;font-size:12px"
                                                                                   title="Logo" height="60"></td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td align="center" style="padding:0;Margin:0"><p
                                                                                  style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:21px;color:#2D3142;font-size:13px">
                                                                              <a target="_blank"
                                                                                 style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:none;color:#2D3142;font-size:14px"
                                                                                 href="{{configuration.PrivatePolicyLink}}">{{localizer["PrivacyPolicy"]}}</a>
                                                                      </tr>
                                                                      <tr>
                                                                          <td align="center"
                                                                              style="Margin:0;padding: 20px 0 0;"><p
                                                                                  style="Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:Arial, sans-serif;line-height:21px;color:#2D3142;font-size:14px">
                                                                              <a target="_blank" href=""
                                                                                 style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#2D3142;font-size:14px"></a>
                                                                              © {{DateTime.UtcNow.Year}}&nbsp;{{configuration.CompanyName}}<a target="_blank" href="{{configuration.HomePageLink}}"
                                                                                                    style="-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;text-decoration:underline;color:#2D3142;font-size:14px"></a>
                                                                          </p></td>
                                                                      </tr>
                                                                  </table>
                                                              </td>
                                                          </tr>
                                                      </table>
                                                  </td>
                                              </tr>
                                          </table>
                                      </td>
                                  </tr>
                              </table>
                          </td>
                      </tr>
                  </table>
              </div>
              </body>
              </html>
              """;
    }
}