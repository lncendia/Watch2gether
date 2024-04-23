namespace PJMS.AuthService.Abstractions.AppEmailService.Structs;

/// <summary>
/// Объект данных об отправляемом Email
/// </summary>
public abstract class EmailData
{
    /// <summary>
    /// Email адрес получателя
    /// </summary>
    public required string Recipient { get; init; }

    /// <summary>
    /// Абстрактный метод, который позволяет посетителю IEmailVisitor посетить объект и выполнить соответствующие действия.
    /// </summary>
    /// <param name="visitor">Посетитель IEmailVisitor.</param>
    public abstract void Accept(IEmailVisitor visitor);
}

/// <summary>
/// Класс, представляющий данные для электронного письма с подтверждением регистрации.
/// </summary>
public class ConfirmRegistrationEmail : EmailData
{
    /// <summary>
    /// Ссылка для подтверждения регистрации.
    /// </summary>
    public required string ConfirmLink { get; init; }

    /// <summary>
    /// Метод, позволяющий посетителю IEmailVisitor посетить текущий объект ConfirmRegistrationEmail и выполнить соответствующие действия.
    /// </summary>
    /// <param name="visitor">Посетитель IEmailVisitor.</param>
    public override void Accept(IEmailVisitor visitor) => visitor.Visit(this);
}

/// <summary>
/// Класс, представляющий данные для электронного письма с подтверждением восстановления пароля.
/// </summary>
public class ConfirmRecoverPasswordEmail : EmailData
{
    /// <summary>
    /// Ссылка для подтверждения восстановления пароля.
    /// </summary>
    public required string ConfirmLink { get; init; }

    /// <summary>
    /// Метод, позволяющий посетителю IEmailVisitor посетить текущий объект ConfirmRecoverPasswordEmail и выполнить соответствующие действия.
    /// </summary>
    /// <param name="visitor">Посетитель IEmailVisitor.</param>
    public override void Accept(IEmailVisitor visitor) => visitor.Visit(this);
}

/// <summary>
/// Класс, представляющий данные для электронного письма с подтверждением сброса 2фа.
/// </summary>
public class TwoFactorCodeEmail : EmailData
{
    /// <summary>
    /// Код подтверждения 2фа.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Метод, позволяющий посетителю IEmailVisitor посетить текущий объект ConfirmResetTwoFactorEmail и выполнить соответствующие действия.
    /// </summary>
    /// <param name="visitor">Посетитель IEmailVisitor.</param>
    public override void Accept(IEmailVisitor visitor) => visitor.Visit(this);
}

/// <summary>
/// Класс, представляющий данные для электронного письма с подтверждением изменения электронной почты.
/// </summary>
public class ConfirmMailChangeEmail : EmailData
{
    /// <summary>
    /// Ссылка для подтверждения изменения электронной почты.
    /// </summary>
    public required string ConfirmLink { get; init; }

    /// <summary>
    /// Метод, позволяющий посетителю IEmailVisitor посетить текущий объект ConfirmMailChangeEmail и выполнить соответствующие действия.
    /// </summary>
    /// <param name="visitor">Посетитель IEmailVisitor.</param>
    public override void Accept(IEmailVisitor visitor) => visitor.Visit(this);
}