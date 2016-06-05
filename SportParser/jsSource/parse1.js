function parse(fs_input, update, odds, action) {
    var dataEventHolder = cjs.dic.get('dataEventHolderProxy').getHolder();
    var dataLeagueHolder = cjs.dic.get('dataLeagueHolderProxy').getHolder();
    var dataParticipantHolder = cjs.dic.get('dataParticipantHolder');
    var feedTranslator = cjs.dic.get('Feed_Translator');
    var translateItem = cjs.dic.get('helperTranslateItem');
    var navigation = cjs.dic.get('list_navigation');
    if (fs_input == null || fs_input.length < 4 || fs_input == '0') {
        u_304 = 'd41d8cd98f00b204e9800998ecf8427e';
        return true
    }
    update = (typeof update == 'undefined' || update == false) ? false : true;
    odds = (typeof odds == 'undefined' || odds == false) ? false : true;
    var eventItem, leagueItem;
    var rows = fs_input.split(JS_ROW_END);
    var rows_length = rows.length;
    var labl_id;
    var parse_sport_id = sport_id;
    var parse_sport = sport;
    var return_val = true;
    var resort_stages = false;
    var top_leagues_switch = true;
    var eventId, tmp;
    var special = false;
    var LEAGUE_INDEX = 'ZA';
    var SPORT_INDEX = 'SA';
    var EVENT_INDEX = 'AA';
    var MOVED_EVENTS_INDEX = 'QA';
    var TOP_LEAGUES_INDEX = 'SG';
    var U_304_INDEX = 'A1';
    var REFRESH_UTIME_INDEX = 'A2';
    var DOWNLOAD_UL_FEED_INDEX = 'UL';
    var PAST_FUTURE_GAMES_INDEX = 'FG';
    var PARTICIPANT_INDEX = 'PR';
    var SPECIAL_INDEX = 'ST';
    var isMyTeamsAction = (typeof cjs.myTeams !== 'undefined' && action === cjs.myTeams.FEED_ACTION);
    var isRepairAction = (action == 'repair' || action == 'frepair');
    if (!update && !odds && !isRepairAction) {
        if (rows_length == 1) {
            rows_length = 0
        }
        if (!isDetailGetter() && (typeof action == 'undefined' || !isMyTeamsAction)) {
            preload_show()
        }
    }
    for (var i = 0; i < rows_length; i++) {
        var row = rows[i].split(JS_CELL_END);
        var row_length = row.length - 1;
        var index = row[0].split(JS_INDEX);
        var indexName, indexValue;
        if (typeof index[0] !== 'undefined') {
            indexName = index[0]
        }
        if (typeof index[1] !== 'undefined') {
            indexValue = index[1]
        }
        if (indexName === SPORT_INDEX) {
            parse_sport_id = indexValue;
            parse_sport = SPORT_LIST_BY_ID[parse_sport_id];
            parsed_sports[parse_sport_id] = parse_sport;
            continue
        } else if (indexName === LEAGUE_INDEX) {
            var tmp_labl = {};
            var backupedLeagueItem = cjs.dic.getNewInstance('Data_LeagueItem');
            if (parse_sport_id == cjs.constants.sport.GOLF || isNoDuelSport(parse_sport_id)) {
                tmp_labl['AB'] = '';
                tmp_labl['AC'] = '';
                tmp_labl['AD'] = ''
            }
            for (var j = 0; j < row_length; j++) {
                var rowParts = row[j].split(JS_INDEX    );
                if (rowParts.length == 2) {
                    tmp_labl[rowParts[0]] = rowParts[1]
                }
            }
            tmp_labl['display'] = tmp_labl.ZD != 'c';
            tmp_labl['g_count'] = 0;
            tmp_labl['sport_id'] = parse_sport_id;
            tmp_labl['sport'] = parse_sport;
            labl_id = parse_sport_id + '_' + tmp_labl.ZC;
            if (isMyTeamsAction) {
                var mgLeagueData = cjs.mygames.getLabels();
                if (mgLeagueData[labl_id] != null) {
                    tmp_labl['g_count'] = mgLeagueData[labl_id]['g_count']
                }
            }
            if (typeof cjs.myLeagues != 'undefined') {
                if (cjs.myLeagues.isTop(labl_id) && !top_leagues_switch) {
                    resort_stages = true
                }
                top_leagues_switch = cjs.myLeagues.isTop(labl_id)
            }
            if (dataLeagueHolder.hasLeague(labl_id)) {
                var backupedLeagueItemData = dataLeagueHolder.getLeague(labl_id).getData();
                backupedLeagueItem.reinit(backupedLeagueItemData);
                tmp_labl['g_count'] = backupedLeagueItem.getEventCount()
            }
            if (isRepairAction) {
                if (dataLeagueHolder.hasLeague(labl_id)) {
                    leagueItem = dataLeagueHolder.getLeague(labl_id);
                    for (var key in tmp_labl) {
                        if (key.length > 2 || (key == "ZA" && tmp_labl[key] == '')) {
                            continue
                        }
                        leagueItem.setValue(key, tmp_labl[key])
                    }
                }
            } else if (!update) {
                leagueItem = dataLeagueHolder.getOrCreateNewLeague(labl_id);
                leagueItem.reinit(tmp_labl)
            } else if (update && labl_id) {
                if (dataLeagueHolder.hasLeague(labl_id)) {
                    leagueItem = dataLeagueHolder.getLeague(labl_id);
                    for (var key in tmp_labl) {
                        if (key.length > 2 || (key == "ZA" && tmp_labl[key] == '') || tmp_labl[key] == leagueItem.getValue(key)) {
                            continue
                        }
                        fs_update.label_set_property(labl_id, key, tmp_labl[key]);
                        leagueItem.setValue(key, tmp_labl[key])
                    }
                }
            }
            if (leagueItem) {
                leagueItem = translateItem.translate(leagueItem, feedTranslator, backupedLeagueItem)
            }
        } else if (indexName === MOVED_EVENTS_INDEX) {
            for (var j = 0; j < row_length; j++) {
                switch (row[j].substr(0, 2)) {
                    case 'QB':
                        eventId = 'g_' + parse_sport_id + '_' + row[j].substr(3);
                        break;
                    case 'QC':
                        if (eventId) {
                            tmp = (row[j].substr(3) + "").split('|');
                            fsEventsUpdatedStartTime[eventId] = {
                                start_time: parseInt(tmp[0]),
                                end_time: tmp[1] ? parseInt(tmp[1]) : null
                            };
                            eventId = null
                        }
                        break
                }
            }
            continue
        } else if (indexName === TOP_LEAGUES_INDEX && project_type_name == '_portable') {
            tmp = indexValue;
            if (tmp) {
                tmp = tmp.split(';');
                for (var j = 0, _len = tmp.length; j < _len; j++) {
                    cjs.myLeagues._topLeagues[parse_sport_id + '_' + tmp[j]] = {}
                }
            }
            continue
        } else if (indexName === PARTICIPANT_INDEX) {
            for (var j = 0; j < row_length; j++) {
                var tmpIndex = row[j].split(JS_INDEX);
                var key = tmpIndex[0];
                var value = tmpIndex[1];
                if (key == 'PR') {
                    var participantData = value.split('|');
                    var participantId = participantData[0];
                    var participantItem = dataParticipantHolder.getOrCreateNewParticipant(participantId);
                    participantItem.reinit(participantData)
                } else if (key == 'LV') {
                    feedTranslator.parseIntoDictionary(value)
                }
            }
            continue
        } else if (indexName === SPECIAL_INDEX && indexValue === 'repair') {
            special = true;
            var repairRows = [];
            for (var j = i + 1; j < rows_length; j++) {
                repairRows.push(rows[j]);
                i++;
                if (rows[j].split(JS_CELL_END)[0].split(JS_INDEX)[0] === 'A1') {
                    break
                }
            }
            continue
        } else if (indexName === U_304_INDEX) {
            if (typeof action != 'undefined' && action == 'update') {
                u_304 = indexValue
            }
            rows_length--;
            continue
        } else if (indexName === REFRESH_UTIME_INDEX) {
            var tmp_refresh_utime = indexValue - 0;
            if (tmp_refresh_utime > refresh_utime) {
                refresh_utime = tmp_refresh_utime;
                return_val = false
            }
        } else if (indexName === DOWNLOAD_UL_FEED_INDEX) {
            tmp = indexValue - 0;
            if (tmp) {
                var feedService = cjs.feedService[cjs.Feed_Service_LocalUpdate.NAME];
                if (feedService) {
                    feedService.setSyncTime(tmp)
                }
            }
        } else if (indexName === PAST_FUTURE_GAMES_INDEX) {
            setGamePlanSettings(indexValue.split(";"))
        } else if (indexName === EVENT_INDEX) {
            var original_id = indexValue;
            var id = 'g_' + parse_sport_id + '_' + original_id;
            var backupedEventItem = cjs.dic.getNewInstance('Data_EventItem');
            var eventItemExists = dataEventHolder.hasEvent(id);
            if ((update || isRepairAction || odds) && !eventItemExists) {
                continue
            }
            if (eventItemExists) {
                var backupedEventItemData = dataEventHolder.getItem(id).getData();
                backupedEventItem.reinit(backupedEventItemData)
            }
            eventItem = dataEventHolder.getOrCreateNewEvent(id);
            if (!update && !odds && !isRepairAction) {
                if (!navigation.isMyGames() || !isMyTeamsAction) {
                    leagueItem.setValue('g_count', leagueItem.getEventCount() + 1)
                }
                eventItem.reinit(createDefaultMatchItem(parse_sport));
                eventItem.setValue('original_id', original_id);
                eventItem.setValue('labl_id', labl_id);
                eventItem.setValue('sport_id', parse_sport_id);
                eventItem.setValue('sport', parse_sport)
            }
            for (var j = 1; j < row_length; j++) {
                var rowParts = row[j].split(JS_INDEX);
                if (rowParts.length != 2) {
                    continue
                }
                var key = rowParts[0];
                var new_value_string = rowParts[1];
                if (update && (key == 'YA' || key == 'YB' || key == 'YC' || key == 'YD' || key == 'YE' || key == 'YF' || key == 'YG' || key == 'YI' || key == 'YJ' || key == 'YL' || key == 'YM')) {
                    continue
                }
                if (key == 'EA' || key == 'EB' || key == 'EC' || key == 'ED') {
                    continue
                }
                var new_value = new_value_string;
                if ($.inArray(key, ['ND', 'NG']) === -1) {
                    new_value -= 0
                }
                if (isNaN(new_value) || new_value_string == '') {
                    new_value = new_value_string
                }
                if (update || odds) {
                    var swap_corrected = get_index_and_value_for_swapped(id, key, new_value);
                    key = swap_corrected.key;
                    new_value = swap_corrected.value
                }
                if (key == "AG" || key == "AH" || key.match(/^B[A-H]$/)) {
                    new_value = fs_update.update_merged_score(eventItem, key, new_value)
                }
                if (update && eventItem.getValue(key) !== new_value) {
                    fs_update.property_set(id, key, new_value, eventItem.getValue(key))
                }
                eventItem.setValue(key, new_value)
            }
            if (action == 'update') {
                for (var j = 1; j < row_length; j++) {
                    var key = row[j].substr(0, 2);
                    if (key == 'EA' || key == 'EB' || key == 'EC' || key == 'ED') {
                        var new_value_arr = row[j].substr(3).split(',');
                        var incidentTime = new_value_arr[1] / 60;
                        var updatedTime = new_value_arr[2];
                        var counterTime = getCounterTime(id);
                        var interval = 5;
                        if (new_value_arr[0] == eventItem.getStage() && incidentTime >= counterTime - interval && incidentTime <= counterTime + interval) {
                            new_value = updatedTime;
                            if (eventItem.getValue(key) !== new_value) {
                                fs_update.property_set(id, key, new_value, eventItem.getValue(key));
                                eventItem.setValue(key, new_value)
                            }
                        }
                    } else if (sport_id == cjs.constants.sport.GOLF || isNoDuelSport(sport_id)) {
                        var new_value_arr = row[j].substr(3).split(',');
                        if ($.inArray(key, ['AB', 'AC', 'AD']) !== -1) {
                            fs_update.label_set_property(eventItem.getValue('labl_id'), key, new_value_arr)
                        }
                    }
                }
            }
            if (!odds) {
                eventItem.setValue('counter', TXT_SPORT[parse_sport][eventItem.getStage()])
            }
            eventItem = translateItem.translate(eventItem, feedTranslator, backupedEventItem);
            var check_start_times = category != 5 && typeof action == 'undefined' && !odds && !update && !country && !tournament;
            var removeEventByTime = check_start_times && !check_start_time(eventItem.getStartUTime(), eventItem.getEndUTime()) && !eventItem.isLive();
            var removeEventByInvalidLeague = !eventItem.getLeague().isValid();
            if (removeEventByTime || !eventItem.isValid() || removeEventByInvalidLeague) {
                if (!navigation.isMyGames() || !isMyTeamsAction) {
                    leagueItem.setValue('g_count', leagueItem.getEventCount() - 1)
                }
                dataEventHolder.removeEvent(id);
                fs_update.removeEvent(id)
            }
            if (!update && !odds && !special) {
                if (typeof participantItem != 'undefined' && eventItem.isValid()) {
                    participantItem.addEventId(eventItem.getId())
                }
            }
        }
    }
    fs_input = null;
    if (special && repairRows.length > 0) {
        parse(repairRows.join(JS_ROW_END), false, false, "frepair")
    }
    var leaguesInHolder = dataLeagueHolder.getReferences();
    for (var leagueId in leaguesInHolder) {
        if (!leaguesInHolder[leagueId].isValid()) {
            dataLeagueHolder.removeLeague(leagueId)
        }
    }
    return return_val
}
