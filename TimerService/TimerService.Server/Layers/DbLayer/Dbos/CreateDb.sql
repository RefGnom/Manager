CREATE TABLE IF NOT EXISTS timer_sessions
(
    "Id"        uuid                     NOT NULL,
    "TimerId"   uuid                     NOT NULL,
    "StartTime" timestamp with time zone NOT NULL,
    "StopTime"  timestamp with time zone,
    "IsOver"    boolean                  NOT NULL,
    CONSTRAINT "PK_timer_sessions" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS timers
(
    "Id"          uuid                     NOT NULL,
    "UserId"      uuid                     NOT NULL,
    "Name"        text                     NOT NULL,
    "StartTime"   timestamp with time zone NOT NULL,
    "PingTimeout" interval,
    "Status"      integer                  NOT NULL,
    CONSTRAINT "PK_timers" PRIMARY KEY ("Id")
);