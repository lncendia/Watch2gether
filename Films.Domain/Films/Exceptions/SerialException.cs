using System;

namespace Films.Domain.Films.Exceptions;

public class SerialException() : Exception("For the series, you must specify the number of seasons and episodes");