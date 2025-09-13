create table if not exists works
(
    id                  uuid                     NOT NULL,
    "recipientId"       uuid                     NOT NULL,
    title               text                     NOT NULL,
    description         text,
    status              integer                  NOT NULL,
    "createdUtc"        timestamp with time zone NOT NULL,
    "deadLineUtc"       timestamp with time zone,
    "reminderIntervals" interval[],
    CONSTRAINT "PK_works" PRIMARY KEY (id)
);
