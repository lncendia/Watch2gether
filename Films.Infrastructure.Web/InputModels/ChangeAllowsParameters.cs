﻿using System.ComponentModel.DataAnnotations;

namespace Films.Infrastructure.Web.Contracts.Settings;

public class ChangeAllowsParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Звуковой сигнал")]
    public bool Beep { get; init; } = true;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Скример")]
    public bool Scream { get; init; } = true;
        
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Изменение имени")]
    public bool Change { get; init; } = true;
}