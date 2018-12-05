require 'time'

class Guard
  attr_reader :id
  attr_reader :actions

  def initialize(id)
    @id = id
    @actions = []
  end

  def most_common_minute_slept
    minute_sleep_counter.sort_by { |k, v| -v }.first
  end

  def minute_sleep_counter
    (0...60).map { |minute| [ minute, minute_ranges_slept.count { |r| r.include?(minute) } ] }.to_h
  end

  def minutes_slept
    sleep_actions.each_with_index.map { |s, i| wake_actions[i].timestamp.minute - s.timestamp.minute }.sum
  end

  def minute_ranges_slept
    sleep_actions.each_with_index.map { |s, i| s.timestamp.minute...wake_actions[i].timestamp.minute }
  end

  private
    def sleep_actions
      @actions.select { |a| a.action == :sleep }.sort { |a| a.timestamp.minute }
    end

    def wake_actions
      wake_action = @actions.select { |a| a.action == :wake }.sort { |a| a.timestamp.minute }
    end
end

class GuardAction
  attr_reader :action
  attr_reader :timestamp

  def initialize(action, timestamp)
    @action = action
    @timestamp = timestamp
  end
end

def parse_time(line)
  DateTime.parse(line.split('[')[1].split(']')[0])
end

lines = File.readlines('input.txt')
guards = []
guard = nil
lines.sort { |a, b| parse_time(a) <=> parse_time(b) }.each_with_index do |line, i|
  timestamp = parse_time(line)
  description = line.split(']')[1].strip

  if description.start_with?('Guard')
    guards << guard unless guard.nil?

    id = Integer(description.split('#')[1].split(' ')[0])
    guard = guards.find { |g| g.id == id } || Guard.new(id)
    guard.actions << GuardAction.new(:begin, timestamp)
  else
    if description == 'falls asleep'
      action = :sleep
    else
      # wakes up
      action = :wake
    end
    guard.actions << GuardAction.new(action, timestamp)
  end
end

most_rested_guard = guards.max{ |a, b| a.minutes_slept <=> b.minutes_slept }
common_minute = most_rested_guard.most_common_minute_slept[0]
puts "Guard ##{most_rested_guard.id} slept the most,
especially at minute #{common_minute}
Answer 1: #{most_rested_guard.id * common_minute}\n"

most_consistent_guard = guards.max { |a, b| a.most_common_minute_slept[1] <=> b.most_common_minute_slept[1] }
puts "Guard ##{most_consistent_guard.id} slept at minute #{most_consistent_guard.most_common_minute_slept[0]}
#{most_consistent_guard.most_common_minute_slept[1]} times, more than any other guard.
Answer 2: #{most_consistent_guard.id * most_consistent_guard.most_common_minute_slept[0]}"