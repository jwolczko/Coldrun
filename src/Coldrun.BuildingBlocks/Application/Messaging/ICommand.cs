using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.BuildingBlocks.Application.Messaging;

public interface ICommand
{
}

public interface ICommand<out TResponse>
{
}
